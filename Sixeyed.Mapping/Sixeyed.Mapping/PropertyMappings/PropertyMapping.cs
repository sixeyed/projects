using System;
using System.Linq.Expressions;
using System.Reflection;
using Sixeyed.Mapping.Extensions;
using Sixeyed.Mapping.PropertyMappings.DelegateWrappers;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Base class for property mappings
    /// </summary>
    /// <typeparam name="TSource">Type of the source object</typeparam>
    public abstract class PropertyMapping<TSource> : IPropertyMapping
    {
        private Delegate _conversion;
        private LambdaExpression _targetExpression;
        private Delegate _targetAccessor;

        /// <summary>
        /// Gets the name of the target item to be mapped
        /// </summary>
        public string TargetName
        {
            get { return TargetPropertyInfo.Name; }
        }

        /// <summary>
        /// Gets/sets the source object
        /// </summary>
        public TSource Source { get; set; }
        
        /// <summary>
        /// Gets the property to be populated
        /// </summary>
        public PropertyInfo TargetPropertyInfo { get; set; }

      
        /// <summary>
        /// Constructor with source object and target property
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="source">Target property</param>
        public PropertyMapping(TSource source, PropertyInfo target)
        {
            Source = source;
            TargetPropertyInfo = target;          
        }

        /// <summary>
        /// Constructor with source object and target accessor
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="targetExpression">Target accessor</param>
        public PropertyMapping(TSource source, LambdaExpression targetExpression)
        {
            Source = source;
            TargetPropertyInfo = targetExpression.AsPropertyInfo();
            if (targetExpression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpression = targetExpression.Body as MemberExpression;
                //for mappings with nested properties, e.g.
                //  t => t.Address.Line1
                //if the preceeding expression is a parameter, then this is not
                //a nested expression, so we can use the ParameterInfo directly
                //otherwise we need to execute the func to get to the object
                //to set the value of:
                if (memberExpression.Expression.NodeType != ExpressionType.Parameter)
                {
                    var parameter = memberExpression.Expression.GetParameterExpression();
                    if (parameter != null)
                    {
                        _targetExpression = Expression.Lambda(memberExpression.Expression, parameter);
                    }
                }
            }
        }        

        /// <summary>
        /// Whether the source field can be read
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public abstract bool CanRead(object source);

        /// <summary>
        /// Whether the target can be written to
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool CanWrite(object target)
        {
            return target != null && TargetPropertyInfo.CanWrite;
        }        

        /// <summary>
        /// Gets the source property value
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public object GetValue(object source)
        {
            object value = GetValueInternal(source);
            if (_conversion != null)
            {
                value = _conversion.DynamicInvoke(value);
            }
            return value;
        }

        /// <summary>
        /// Maps the value from source to target
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void MapValue(object source, object target)
        {
            object sourceValue = GetValue(source);
            SetValue(target, sourceValue);
        }

        /// <summary>
        /// Sets the target property value
        /// </summary>
        /// <param name="target"></param>
        /// <param name="sourceValue"></param>
        public void SetValue(object target, object sourceValue)
        {
            sourceValue = ConvertIfNecessary(sourceValue, TargetPropertyInfo.PropertyType);
            object realTarget = target;
            //if the real target is a property of a nested object, 
            //then execute the expression to get the nested object:
            if (_targetExpression != null)
            {
                if (_targetAccessor == null)
                {
                    _targetAccessor = _targetExpression.Compile();
                }
                realTarget = _targetAccessor.DynamicInvoke(target);
            }
            SetDelegateWrapper.SetValue(realTarget, sourceValue, TargetPropertyInfo);
        }
        
        /// <summary>
        /// Set the conversion to use during mapping
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="conversion"></param>
        public void SetConversion<TInput, TOutput>(Func<TInput, TOutput> conversion)
        {
            _conversion = conversion;
        }

        /// <summary>
        /// Get the source field value from the source object
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected abstract object GetValueInternal(object source);        

        private object ConvertIfNecessary(object sourceValue, Type type)
        {
            if (sourceValue.GetType() == type)
                return sourceValue;
            if (sourceValue.GetType().IsAssignableFrom(type))
                return sourceValue;
            if (sourceValue is IConvertible & type.GetInterface("IConvertible") != null)
                return Convert.ChangeType(sourceValue, type);
            //this covers most things; odd ones include Guid:
            if (sourceValue is string & type == typeof(Guid))
                return new Guid((string)sourceValue);
            return sourceValue;
        }
    }
}