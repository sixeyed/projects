using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Automap for populating an object from a delimited field input, populated as <see cref="string"/>
    /// </summary>
    /// <remarks>
    /// By default, uses <see cref="DataMemberOrderMatchingStrategy"/>, matching target properties flagged with a 
    /// <see cref="DataMemberAttribute"/>, so the order of the target data member is used as the source field index
    /// </remarks>
    /// <typeparam name="TTarget">Type of target object</typeparam>
    public class CsvAutoMap<TTarget> : CsvMap<TTarget>
        where TTarget : class, new()
    {       
        /// <summary>
        /// Whether to automatically matach & map unspecified target properties
        /// </summary>
        /// <remarks>
        /// Required by map base, in AutoMap always returns true
        /// </remarks>
        public override bool AutoMapUnspecifiedTargets
        {
            get { return true; }
            set { } //do nothing
        }

        /// <summary>
        /// Specify the field delimiter for the input source (defaults to comma)
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public new CsvAutoMap<TTarget> WithFieldDelimiter(char delimiter)
        {
            return (CsvAutoMap<TTarget>)base.WithFieldDelimiter(delimiter);
        }

        /// <summary>
        /// Specify the caching strategy for the map, used to cache the property match between source and target
        /// </summary>
        /// <typeparam name="TCachingStrategy"></typeparam>
        /// <returns></returns>
        public new CsvAutoMap<TTarget> Cache<TCachingStrategy>()
            where TCachingStrategy : ICachingStrategy, new()
        {
            return (CsvAutoMap<TTarget>)base.Cache<TCachingStrategy>();
        }

        /// <summary>
        /// Specify the matching strategy for the map, used to match source and target properties
        /// </summary>
        /// <typeparam name="TMatchingStrategy"></typeparam>
        /// <returns></returns>
        public new CsvAutoMap<TTarget> Matching<TMatchingStrategy>()
            where TMatchingStrategy : IMatchingStrategy<int>, new()
        {
            return (CsvAutoMap<TTarget>)base.Matching<TMatchingStrategy>();
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="sourceFieldIndex">Index of the source field to get</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <returns></returns>
        public new CsvAutoMap<TTarget> Specify(int sourceFieldIndex, Expression<Func<TTarget, object>> targetPropertyExpression)
        {
            return (CsvAutoMap<TTarget>)base.Specify(sourceFieldIndex, targetPropertyExpression);
        }

        /// <summary>
        /// Specify an explicit property mapping with a value conversion
        /// </summary>
        /// <typeparam name="TInput">Source property type</typeparam>
        /// <typeparam name="TOutput">Target property type</typeparam>
        /// <param name="sourceFieldIndex">Index of the source field to get</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <param name="conversion">Conversion function to apply to the source value</param>
        /// <returns></returns>
        public new CsvAutoMap<TTarget> Specify<TInput, TOutput>(int sourceFieldIndex, Expression<Func<TTarget, TOutput>> targetPropertyExpression, Func<TInput, TOutput> conversion)
        {
            return (CsvAutoMap<TTarget>)base.Specify<TInput, TOutput>(sourceFieldIndex, targetPropertyExpression, conversion);
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="mappingAction">Action to populate target property value</param>
        /// <returns></returns>
        public new CsvAutoMap<TTarget> Specify(Action<string, TTarget> mappingAction)
        {
            return (CsvAutoMap<TTarget>)base.Specify(mappingAction);
        }

        /// <summary>
        /// Populate an existing target object from a source string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void PopulateTarget(string source, TTarget target)
        {
            var map = new CsvAutoMap<TTarget>();
            map.Populate(source, target);
        }

        /// <summary>
        /// Create a target object populated from a source string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget CreateTarget(string source)
        {
            var map = new CsvAutoMap<TTarget>();
            return map.Create(source);
        }

        /// <summary>
        /// Create a collection of target objects from a collection of source strings
        /// </summary>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public static List<TTarget> CreateTargetList(IList<string> sourceList)
        {
            var map = new CsvAutoMap<TTarget>();
            return map.CreateList(sourceList);
        }
    }
}
