using System;
using System.Linq.Expressions;
using Sixeyed.Mapping.MappingStrategies;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Base class for populating an object from a delimited field input, populated as <see cref="string"/>
    /// </summary>
    /// <remarks>
    /// By default, uses <see cref="DataMemberOrderMatchingStrategy"/>, matching target properties flagged with a 
    /// <see cref="DataMemberAttribute"/>, so the order of the target data member is used as the source field index
    /// </remarks>
    /// <typeparam name="TTarget">Type of target object</typeparam>
    public abstract class CsvMap<TTarget> : Map<string, TTarget, int>
        where TTarget : class, new()
    {
        private char _fieldDelimiter = ',';

        protected override IMappingStrategy<int> GetDefaultMappingStrategy()
        {
            var mapping = new CsvMappingStrategy();
            mapping.FieldDelimiter = _fieldDelimiter;
            return mapping;
        }

        protected override IMatchingStrategy<int> GetDefaultMatchingStrategy()
        {
            return new DataMemberOrderMatchingStrategy();
        }

        /// <summary>
        /// Specify the field delimiter for the input source (defaults to comma)
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public CsvMap<TTarget> WithFieldDelimiter(char delimiter)
        {
            _fieldDelimiter = delimiter;
            if (MappingStrategy != null & MappingStrategy is CsvMappingStrategy)
            {
                ((CsvMappingStrategy)MappingStrategy).FieldDelimiter = _fieldDelimiter;
            }
            return this;
        }

        /// <summary>
        /// Specify the caching strategy for the map, used to cache the property match between source and target
        /// </summary>
        /// <typeparam name="TCachingStrategy"></typeparam>
        /// <returns></returns>
        public new CsvMap<TTarget> Cache<TCachingStrategy>()
            where TCachingStrategy : ICachingStrategy, new()
        {
            return (CsvMap<TTarget>)base.Cache<TCachingStrategy>();
        }

        /// <summary>
        /// Specify the matching strategy for the map, used to match source and target properties
        /// </summary>
        /// <typeparam name="TMatchingStrategy"></typeparam>
        /// <returns></returns>
        public new CsvMap<TTarget> Matching<TMatchingStrategy>()
            where TMatchingStrategy : IMatchingStrategy<int>, new()
        {
            return (CsvMap<TTarget>)base.Matching<TMatchingStrategy>();
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="sourceFieldIndex">Index of the source field to get</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <returns></returns>
        public CsvMap<TTarget> Specify(int souceFieldIndex, Expression<Func<TTarget, object>> targetPropertyExpression)
        {
            var mapping = new CsvFieldMapping(souceFieldIndex, targetPropertyExpression);
            MappingStrategy.AddMapping(mapping);
            return this;
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
        public CsvMap<TTarget> Specify<TInput, TOutput>(int souceFieldIndex, Expression<Func<TTarget, TOutput>> targetPropertyExpression, Func<TInput, TOutput> conversion)
        {
            var mapping = new CsvFieldMapping(souceFieldIndex, targetPropertyExpression);
            mapping.SetConversion<TInput, TOutput>(conversion);
            MappingStrategy.AddMapping(mapping);
            return this;
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="mappingAction">Action to populate target property value</param>
        /// <returns></returns>
        public new CsvMap<TTarget> Specify(Action<string, TTarget> mappingAction)
        {
            return (CsvMap<TTarget>)base.Specify(mappingAction);
        }
    }
}
