using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpectedObjects
{
    public class ComparisonStrategy<T,TProperty> : ComparisonStrategy<T>
    {
        readonly Func<TProperty, TProperty, bool> _comparisonFunction;
        readonly Expression<Func<T, TProperty>> _propertySelector;

        public ComparisonStrategy(Expression<Func<T,TProperty>> propertySelector, Func<TProperty,TProperty,bool> comparisonFunction)
        {
            _propertySelector = propertySelector;
            _comparisonFunction = comparisonFunction;
        }

        public override bool Compare(T firstObject, T secondObject)
        {
            var body = _propertySelector.Body as MemberExpression;

            if (body == null)
            {
                var ubody = (UnaryExpression)_propertySelector.Body;
                body = ubody.Operand as MemberExpression;
            }

            if (body == null || body.Member.MemberType != MemberTypes.Property) throw new UnsupportedExpressionType(_propertySelector.Body.NodeType);

            var property = (PropertyInfo)body.Member;
            var propertyGetMethod = property.GetGetMethod();

            return _comparisonFunction.Invoke((TProperty)propertyGetMethod.Invoke(firstObject.GetTarget(_propertySelector), new object[0]),
                                              (TProperty)propertyGetMethod.Invoke(secondObject.GetTarget(_propertySelector), new object[0]));
        }
    }

    public abstract class ComparisonStrategy<T>
    {
        public abstract bool Compare(T firstObject, T secondObject);
    }
}