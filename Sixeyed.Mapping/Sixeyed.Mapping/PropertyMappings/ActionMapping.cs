using System;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Property mapping using an action to map from source to target
    /// </summary>
    /// <typeparam name="TSource">Type of source object</typeparam>
    /// <typeparam name="TTarget">Type of target object</typeparam>
    [Serializable]
    public class ActionMapping<TSource, TTarget> : IPropertyMapping
    {        
        private Action<TSource, TTarget> _mapping;

        /// <summary>
        /// Gets the name of the target item to be mapped
        /// </summary>
        public string TargetName { get; private set; }

        /// <summary>
        /// Constructor with mapping action
        /// </summary>
        /// <param name="mapping">Action to map from source to target</param>
        public ActionMapping(Action<TSource, TTarget> mapping)
        {
            _mapping = mapping;
            TargetName = mapping.Method.Name;
        }

        /// <summary>
        /// Whether the source field can be read
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool CanRead(object source)
        {
            return (_mapping != null & source is TSource);
        }

        /// <summary>
        /// Whether the target can be written to
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool CanWrite(object target)
        {
            return (_mapping != null & target is TTarget);
        }

        /// <summary>
        /// Maps the target value from the source, using the given action
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void MapValue(object source, object target)
        {
            _mapping((TSource)source, (TTarget)target);
        }


        /// <summary>
        /// Set the conversion to use during mapping
        /// </summary>
        /// <remarks>
        /// For compatibility, method does nothing
        /// </remarks>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="conversion"></param>
        public void SetConversion<TInput, TOutput>(Func<TInput, TOutput> conversion)
        {
            //do nothing
        }
    }
}