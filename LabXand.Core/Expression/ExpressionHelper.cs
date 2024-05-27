using LabXand.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace LabXand.Extensions
{
    public class ExpressionHelper
    {
        public static object GetPropertyValue<T>(T objectInstance, string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            return Expression.Lambda<Func<T, object>>(Expression.Convert(GetMemberExpression<T>(parameter, propertyName), typeof(object)), parameter).Compile().Invoke(objectInstance);
        }
        public static MemberExpression GetMemberExpression<T>(string members)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            return GetMemberExpression<T>(parameter, members);
        }

        public static MemberExpression GetMemberExpression<T>(Expression expression, string members)
        {
            return GetMemberAccessExpression(expression, members, typeof(T));
        }

        public static Expression CreateConditionalExpression(Expression instanceExpression, string memberAccess, Type objectType, object value, Type valueType, IConditionalExpressionBuilder conditionalExpressionBuilder)
        {
            List<string> memberList = memberAccess.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string firstMember = memberList.First();
            MemberExpression memberExpression = CreateMemberAccessExpression(objectType, instanceExpression, firstMember);
            Expression resultExpression = null;

            string newMemberAccess = memberAccess.Replace(string.Format("{0}.", firstMember), ""); ;

            if (memberList.Count == 1)
                resultExpression = conditionalExpressionBuilder.Get(memberExpression, value, valueType);
            else
                resultExpression = memberExpression;

            foreach (var member in memberList.Skip(1))
            {
                PropertyInfo property = (PropertyInfo)memberExpression.Member;
                if (property.PropertyType.IsEnumerable())
                {
                    Type genericType = property.PropertyType.GetGenericArguments().First();
                    ParameterExpression newParameter = Expression.Parameter(genericType, string.Format("{0}_param", property.Name));
                    Expression newExpression = CreateConditionalExpression(newParameter, newMemberAccess, property.PropertyType.GetGenericArguments().First(), value, valueType, conditionalExpressionBuilder);

                    Expression callEnyMethod = Expression.Lambda(newExpression, newParameter);
                    return Expression.Call(TypeHelper.GetEnumerableAnyMethod(genericType), memberExpression, callEnyMethod);
                }
                else
                {
                    memberExpression = CreateMemberAccessExpression(property.PropertyType, memberExpression, member);
                    if (newMemberAccess.IndexOf(".") == -1)
                        resultExpression = conditionalExpressionBuilder.Get(memberExpression, value, valueType);
                }
                newMemberAccess = newMemberAccess.Replace(string.Format("{0}.", member), "");
            }
            return resultExpression;
        }

        public static MemberExpression GetMemberAccessExpression(Expression expression, string members, Type type)
        {
            List<string> memberList = members.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string firstMember = memberList.First();
            MemberExpression memberExpression = CreateMemberAccessExpression(type, expression, firstMember);

            foreach (var item in memberList.Skip(1))
            {
                memberExpression = CreateMemberAccessExpression(((PropertyInfo)(memberExpression.Member)).PropertyType, memberExpression, item);
            }
            return memberExpression;
        }

        public static MemberExpression CreateMemberAccessExpression(Type type, string member, ParameterExpression parameter)
        {
            return CreateMemberAccessExpression(type, parameter, member);
        }

        public static MemberExpression CreateMemberAccessExpression(Type type, string member)
        {
            Expression expression = Expression.Parameter(type);
            return CreateMemberAccessExpression(type, expression, member);
        }

        public static MemberExpression CreateMemberAccessExpression(Type type, Expression expression, string member)
        {
            var membersInfo = type.GetMember(member);
            if (membersInfo.Count() > 0)
            {
                MemberInfo memberInfo = membersInfo.First();
                if (memberInfo != null)
                    return Expression.MakeMemberAccess(expression, memberInfo);
                else
                    throw new Exception(string.Format("{0} does not {1} member.", type.Name, member));
            }
            else
                throw new Exception(string.Format("{0} does not {1} member.", type.Name, member));
        }

        public static Expression<Func<T, bool>> CreateEqualCondition<T, ValueType>(string propertyName, object value)
        {
            Type type = typeof(T);
            ParameterExpression parameter = Expression.Parameter(type, "parameter");

            Type valueType = typeof(ValueType);
            ConstantExpression rightExpression = null;

            if (valueType.IsNullable())
                rightExpression = Expression.Constant(value, Nullable.GetUnderlyingType(valueType));
            else
                rightExpression = Expression.Constant(value, valueType);

            MemberExpression propertyAccess = GetMemberExpression<T>(parameter, propertyName);
            if (valueType.IsNullable())
            {
                MemberExpression propertyValueAccess = GetMemberExpression<T>(parameter, string.Format("{0}.Value", propertyName));
                MemberExpression hasValueAccess = GetMemberExpression<T>(parameter, string.Format("{0}.HasValue", propertyName));
                return Expression.Lambda<Func<T, bool>>(
                    Expression.Condition(hasValueAccess, Expression.Equal(propertyValueAccess, rightExpression), Expression.Constant(false)), new[] { parameter });
            }
            else
            {
                return Expression.Lambda<Func<T, bool>>(Expression.Equal(propertyAccess, rightExpression), new[] { parameter });
            }
        }

        public static BinaryExpression CreateEqulityExpression<T>(ParameterExpression parameter, string propertyName, object value)
        {
            //Type type = typeof(T);
            MemberExpression propertyAccessor = ExpressionHelper.GetMemberExpression<T>(propertyName);
            Type type = propertyAccessor.Member.DeclaringType;
            int dotIndexInPropertyName = propertyName.LastIndexOf(".");
            if (dotIndexInPropertyName > 0)
            {
                propertyName = propertyName.Substring(dotIndexInPropertyName + 1);
            }
            //PropertyInfo property = type.GetProperty(propertyName);
            if (propertyAccessor != null)
            {
                Type propertyType = propertyAccessor.Type;
                ConstantExpression rightExpression = null;

                if (propertyType.IsNullable())
                    rightExpression = Expression.Constant(value, Nullable.GetUnderlyingType(propertyType));
                else
                    rightExpression = Expression.Constant(value, propertyType);

                //MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
                if (propertyType.IsNullable())
                {
                    MemberExpression propertyValueAccess = ExpressionHelper.GetMemberExpression<T>(parameter, string.Format("{0}.Value", propertyName));
                    MemberExpression hasValueAccess = ExpressionHelper.GetMemberExpression<T>(parameter, string.Format("{0}.HasValue", propertyName));

                    return Expression.And(hasValueAccess, Expression.Equal(propertyValueAccess, rightExpression));
                }
                else
                {
                    return Expression.Equal(propertyAccessor, rightExpression);
                }
            }
            else
                throw new Exception(string.Format("{0} Property not found in {1} type.", propertyName, type.Name));
        }

        public static BinaryExpression CreateNotEqulityExpression<T>(ParameterExpression parameter, string propertyName, object value)
        {
            Type type = typeof(T);

            PropertyInfo property = type.GetProperty(propertyName);
            if (property != null)
            {
                Type propertyType = property.PropertyType;
                //Type valueType = typeof(ValueType);
                ConstantExpression rightExpression = null;

                if (propertyType.IsNullable())
                    rightExpression = Expression.Constant(value, Nullable.GetUnderlyingType(propertyType));
                else
                    rightExpression = Expression.Constant(value, propertyType);

                MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
                if (propertyType.IsNullable())
                {
                    MemberExpression propertyValueAccess = ExpressionHelper.GetMemberExpression<T>(parameter, string.Format("{0}.Value", propertyName));
                    MemberExpression hasValueAccess = ExpressionHelper.GetMemberExpression<T>(parameter, string.Format("{0}.HasValue", propertyName));

                    return Expression.And(hasValueAccess, Expression.NotEqual(propertyValueAccess, rightExpression));
                }
                else
                {
                    return Expression.NotEqual(propertyAccess, rightExpression);
                }
            }
            else
                throw new Exception(string.Format("{0} Property not found in {1} type.", propertyName, type.Name));
        }

        public static LambdaExpression CreateMemberSelector<T>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            return Expression.Lambda(GetMemberExpression<T>(parameter, propertyName), parameter);
        }

        public static Expression<Func<T, bool>> CreateFromCriteria<T>(Criteria criteria)
        {
            if (criteria != null)
            {
                ParameterExpression parameter = Expression.Parameter(typeof(T));
                return Expression.Lambda<Func<T, bool>>(criteria.GetExpression(parameter), parameter);
            }
            return c => true;
        }
        public static Expression<Func<T, bool>> Rewrite<T>(Expression<Func<T, bool>> exp, ParameterExpression parameter)
        {
            var newExpression = new PredicateRewriterVisitor(parameter).Visit(exp);

            return (Expression<Func<T, bool>>)newExpression;
        }

        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            //Expression<Func<T, dynamic>> expression = p => propertySelectorFunction(p);
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                UnaryExpression unaryExpr = expression.Body as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpression = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpression == null)
                throw new ArgumentException("Parameter expr must be a memberexpression");

            string name = memberExpression.ToString();

            // Remove first parameter
            int index = name.IndexOf(".");
            if (index != -1)
            {
                name = name.Substring(index + 1);
            }

            return name;
        }

        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            //Expression<Func<T, dynamic>> expression = p => propertySelectorFunction(p);
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                UnaryExpression unaryExpr = expression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpression = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpression == null)
                throw new ArgumentException("Parameter expr must be a memberexpression");

            string name = memberExpression.ToString();

            // Remove first parameter
            int index = name.IndexOf(".");
            if (index != -1)
            {
                name = name.Substring(index + 1);
            }

            return name;
        }

        public static string GetNameOfProperty<T, TProperty>(Expression<Func<T, TProperty>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression == null)
            {
                UnaryExpression unaryExpr = propertySelector.Body as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpression = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpression == null)
                throw new ArgumentException("Parameter 'propertySelector' must be a memberexpression");

            string name = memberExpression.ToString();
            name = name.Replace("First().", "").Replace("FirstOrDefault().", "");
            // Remove first parameter
            int index = name.IndexOf(".");
            if (index != -1)
            {
                name = name.Substring(index + 1);
            }

            return name;
        }
    }
}
