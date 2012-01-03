using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Sixeyed.Mapping.Exceptions;
using Sixeyed.Mapping.MappingStrategies;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Base class for populating an object from a database source, populated as <see cref="IDataReader"/>
    /// </summary>
    /// <remarks>
    /// By default, uses <see cref="SimpleNameMatchingStrategy"/>, matching target properties which have
    /// similar names to source data column names. Ignores case and non-aplhanumeric characters
    /// </remarks>
    /// <typeparam name="TTarget">Type of target object</typeparam>
    public abstract class DataReaderMap<TTarget> : Map<IDataReader, TTarget, string>
        where TTarget : class, new()
    {
        protected override IMappingStrategy<string> GetDefaultMappingStrategy()
        {
            return new DataReaderMappingStrategy();
        }

        protected override IMatchingStrategy<string> GetDefaultMatchingStrategy()
        {
            return new SimpleNameMatchingStrategy();
        }

        /// <summary>
        /// Specify the caching strategy for the map, used to cache the property match between source and target
        /// </summary>
        /// <typeparam name="TCachingStrategy"></typeparam>
        /// <returns></returns>
        public new DataReaderMap<TTarget> Cache<TCachingStrategy>()
            where TCachingStrategy : ICachingStrategy, new()
        {
            return (DataReaderMap<TTarget>)base.Cache<TCachingStrategy>();
        }

        /// <summary>
        /// Specify the matching strategy for the map, used to match source and target properties
        /// </summary>
        /// <typeparam name="TMatchingStrategy"></typeparam>
        /// <returns></returns>
        public new DataReaderMap<TTarget> Matching<TMatchingStrategy>()
            where TMatchingStrategy : IMatchingStrategy<string>, new()
        {
            return (DataReaderMap<TTarget>)base.Matching<TMatchingStrategy>();
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="sourceColumnName">Name of the source column to get</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <returns></returns>
        public DataReaderMap<TTarget> Specify(string sourceColumnName, Expression<Func<TTarget, object>> targetPropertyExpression)
        {
            var mapping = new DataReaderColumnMapping(sourceColumnName, targetPropertyExpression);
            MappingStrategy.AddMapping(mapping);
            return this;
        }

        /// <summary>
        /// Specify an explicit property mapping with a value conversion
        /// </summary>
        /// <typeparam name="TInput">Source property type</typeparam>
        /// <typeparam name="TOutput">Target property type</typeparam>
        /// <param name="sourceColumnName">Name of the source column to get</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <param name="conversion">Conversion function to apply to the source value</param>
        /// <returns></returns>
        public DataReaderMap<TTarget> Specify<TInput, TOutput>(string sourceColumnName, Expression<Func<TTarget, TOutput>> targetPropertyExpression, Func<TInput, TOutput> conversion)
        {
            var mapping = new DataReaderColumnMapping(sourceColumnName, targetPropertyExpression);
            mapping.SetConversion<TInput, TOutput>(conversion);
            MappingStrategy.AddMapping(mapping);
            return this;
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="mappingAction">Action to populate target property value</param>
        /// <returns></returns>
        public new DataReaderMap<TTarget> Specify(Action<IDataReader, TTarget> mappingAction)
        {
            return (DataReaderMap<TTarget>)base.Specify(mappingAction);
        }

        /// <summary>
        /// Create a collection of target object populated from a source data reader
        /// </summary>
        /// <remarks>
        /// Takes a collection of source readers for compatibility, but can only read from a single reader. 
        /// Creates a target list and populates it from the source reader - reading from the reader for each 
        /// new target object. Can be called with a data reader which has not already been positioned
        /// </remarks>
        /// <exception cref="MappingException">Thrown if ThrowMappingExceptions is true and more than one data reader is passed in</exception>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public override List<TTarget> CreateList(IList<IDataReader> sourceList)
        {
            if (sourceList.Count != 1)
            {
                if (ThrowMappingExceptions)
                {
                    throw new MappingException("DataReaderMap.CreateList: cannot use zero or multiple IDatReaders, pass a single IDataReader into CreateList");
                }
                return null;
            }
            var list = new List<TTarget>();
            while (sourceList[0].Read())
            {
                list.Add(Create(sourceList[0]));
            }
            return list;
        }
    }
}
