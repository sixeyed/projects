
using System;
namespace Sixeyed.Mapping
{
    /// <summary>
    /// Represents a mapping for a target property 
    /// </summary>
    public interface IPropertyMapping
    {
        /// <summary>
        /// Name of the target
        /// </summary>
        string TargetName { get; }

        /// <summary>
        /// Whether the source field can be read
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        bool CanRead(object source);

        /// <summary>
        /// Whether the target property can be written to
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        bool CanWrite(object target);

        /// <summary>
        /// Sets the conversion to apply to the source value during mapping to the target
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="conversion"></param>
        void SetConversion<TInput, TOutput>(Func<TInput, TOutput> conversion);

        /// <summary>
        /// Maps the target value from the source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        void MapValue(object source, object target);
    }
}