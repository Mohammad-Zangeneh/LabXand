using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace LabXand.Core;

[DataContract]
[KnownType("GetKnownType")]
public abstract class Criteria
{
    [DataMember]
    public virtual object FirstOprand { get; set; }
    [DataMember]
    public virtual object SecondOperand { get; set; }
    public Type ObjectType { get; set; }
    public Expression GetExpression(ParameterExpression parameter)
    {
        if (this.FirstOprand == null && this.SecondOperand == null)
        {
            return null;
        }
        Initialize(parameter);
        return CreateExpression(parameter);
    }

    protected virtual void Initialize(ParameterExpression parameter)
    {            
        if (this.FirstOprand == null)
        {
            throw new Exception("First oprand can not be null.");
        }
        if (this.SecondOperand == null && (GetType() != typeof(IsNullCriteria) && GetType() != typeof(IsNotNullCriteria)))
        {
            throw new Exception("Second operand can not be null.");
        }

        if (this.FirstOprand.GetType() != typeof(string))
        {
            throw new Exception("First operand must be string.");
        }

        if (parameter == null)
        {
            parameter = Expression.Parameter(ObjectType);
        }
        if (ObjectType == null || ObjectType != parameter.Type)
        {
            ObjectType = parameter.Type;
        }
    }
    protected abstract Expression CreateExpression(ParameterExpression parameter);
    public static Type[] GetKnownType()
    {
        return new Type[] { typeof(Criteria), typeof(AndCriteria), typeof(EqualCriteria), typeof(GreaterThan), typeof(GreaterThanOrEqual), typeof(LessThan), typeof(LessThanOrEqual), 
            typeof(LikeCriteria), typeof(NotEqualCriteria), typeof(OrCriteria) , typeof(EmptyCriteria) };
    }
}
