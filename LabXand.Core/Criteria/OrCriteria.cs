using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace LabXand.Core
{
    [DataContract]
    public class OrCriteria : Criteria
    {
        protected override void Initialize(ParameterExpression parameter)
        {
        }
        protected override Expression CreateExpression(ParameterExpression parameter)
        {
            if (base.FirstOprand == null && base.SecondOperand == null)
            {
                return null;
            }

            if (this.SecondOperand != null && this.FirstOprand != null)
            {
                Expression leftExpression = this.FirstOprand as Expression;
                Expression rightExpression = this.SecondOperand as Expression;
                Criteria firstOprand = this.FirstOprand as Criteria;
                Criteria secondOprand = this.SecondOperand as Criteria;

                if (leftExpression == null && rightExpression == null && firstOprand == null && secondOprand == null)
                    throw new Exception("First and second operand must be Criteria or Expression.");

                if (parameter == null)
                {
                    parameter = Expression.Parameter(ObjectType);
                }
                if (ObjectType == null)
                {
                    ObjectType = parameter.Type;
                }

                if (leftExpression == null && firstOprand != null)
                {
                    leftExpression = firstOprand.GetExpression(parameter);
                    if (firstOprand is EmptyCriteria)
                        leftExpression = null;
                }

                if (rightExpression == null && secondOprand != null)
                {
                    rightExpression = secondOprand.GetExpression(parameter);
                    if (secondOprand is EmptyCriteria)
                        rightExpression = null;
                }

                if (base.FirstOprand == null)
                {
                    return rightExpression;
                }

                if (base.SecondOperand == null)
                {
                    return leftExpression;
                }

                if (leftExpression == null && rightExpression == null)
                {
                    return null;
                }

                if (leftExpression == null)
                {
                    return rightExpression;
                }

                if (rightExpression == null)
                {
                    return leftExpression;
                }

                return Expression.Or(leftExpression, rightExpression);
            }
            if (FirstOprand == null)
            {
                try
                {
                    Expression expression = this.SecondOperand as Expression;

                    if (expression == null)
                    {
                        Criteria secondOprand = (Criteria)this.SecondOperand;
                        return secondOprand.GetExpression(parameter);
                    }
                    return expression;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("error occured in {0}. \n message : {1} \n stack trace : {2}", this.GetType().Name, ex.Message, ex.StackTrace), ex);
                }
            }
            if (SecondOperand == null)
            {
                try
                {
                    Expression expression = this.FirstOprand as Expression;

                    if (expression == null)
                    {
                        Criteria firstOprand = (Criteria)this.FirstOprand;
                        return firstOprand.GetExpression(parameter);
                    }
                    return expression;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("error occured in {0}. \n message : {1} \n stack trace : {2}", this.GetType().Name, ex.Message, ex.StackTrace), ex);
                }
            }
            return null;
        }
    }
}

