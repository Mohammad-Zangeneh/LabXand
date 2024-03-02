using System.Linq.Expressions;
using System.Reflection;

namespace LabXand.Core
{
    public abstract class ConditionalExpressionBuilderBase : IConditionalExpressionBuilder
    {
        public Expression Get(MemberExpression memberExpression, object value, Type valueType)
        {
            Type firstdOperandType = ((PropertyInfo)(memberExpression.Member)).PropertyType;
            //Expression rightExpression;
            //if (valueType != firstdOperandType)
            //{
            //    rightExpression = firstdOperandType.FullName.StartsWith(typeof(Nullable<>).FullName) ?
            //        Expression.Convert(rightExpression, Nullable.GetUnderlyingType(firstdOperandType)) :
            //        Expression.Convert(rightExpression, firstdOperandType);
            //}
            value = value.Convert(firstdOperandType.FullName.StartsWith(typeof(Nullable<>).FullName) ? Nullable.GetUnderlyingType(firstdOperandType) : firstdOperandType);
            //value = Convert.ChangeType(value,firstdOperandType.FullName.StartsWith(typeof(Nullable<>).FullName) ? Nullable.GetUnderlyingType(firstdOperandType) : firstdOperandType);
            Expression rightExpression = Expression.Constant(value, firstdOperandType.FullName.StartsWith(typeof(Nullable<>).FullName) ? Nullable.GetUnderlyingType(firstdOperandType) : firstdOperandType);

            if (firstdOperandType.FullName.StartsWith(typeof(Nullable<>).FullName))
            {
                return Expression.And(Expression.NotEqual(memberExpression, Expression.Constant(null)),
                GetExpression(Expression.Convert(memberExpression, Nullable.GetUnderlyingType(firstdOperandType)), rightExpression));
            }
            return GetExpression(memberExpression, rightExpression);

        }
        protected abstract BinaryExpression GetExpression(Expression leftExpression, Expression rightExpression);
    }
}
