using System;
using System.Linq.Expressions;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Property mapping using functions to retrieve source value and set target value
    /// </summary>
    /// <typeparam name="TSource">Type of source object</typeparam>
    /// <typeparam name="TInput">Type of source property value</typeparam>
    /// <typeparam name="TTarget">Type of target object</typeparam> 
    /// <typeparam name="TOutput">Type of target property value</typeparam>
    [Serializable]
    public class FuncMapping<TSource, TInput, TTarget, TOutput> : PropertyMapping<Func<TSource, TInput>>
    {
        /// <summary>
        /// Constructor with source accessor and target accessor
        /// </summary>
        /// <param name="sourceExpression">Source accessor</param>
        /// <param name="targetExpression">Target accessor</param>
        public FuncMapping(Func<TSource, TInput> sourceExpression, Expression<Func<TTarget, TOutput>> targetExpression)
            : base(sourceExpression, targetExpression) { }       

        /// <summary>
        /// Whether the source field can be read
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override bool CanRead(object source)
        {
            return (Source != null & source is TSource);
        }

        /// <summary>
        /// Whether the target can be written to
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override bool CanWrite(object source)
        {
            return (source is TTarget);
        }

        protected override object GetValueInternal(object source)
        {
            return Source((TSource)source);
        }
    }
}