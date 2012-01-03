using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Automap for populating an object from a database source, populated as <see cref="IDataReader"/>
    /// </summary>
    /// <remarks>
    /// By default, uses <see cref="SimpleNameMatchingStrategy"/>, matching target properties which have
    /// similar names to source data column names. Ignores case and non-aplhanumeric characters
    /// </remarks>
    /// <typeparam name="TTarget">Type of target object</typeparam>
    public class DataReaderAutoMap<TTarget> : DataReaderMap<TTarget>
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
        /// Specify the caching strategy for the map, used to cache the property match between source and target
        /// </summary>
        /// <typeparam name="TCachingStrategy"></typeparam>
        /// <returns></returns>
        public new DataReaderAutoMap<TTarget> Cache<TCachingStrategy>()
            where TCachingStrategy : ICachingStrategy, new()
        {
            return (DataReaderAutoMap<TTarget>)base.Cache<TCachingStrategy>();
        }

        /// <summary>
        /// Specify the matching strategy for the map, used to match source and target properties
        /// </summary>
        /// <typeparam name="TMatchingStrategy"></typeparam>
        /// <returns></returns>
        public new DataReaderAutoMap<TTarget> Matching<TMatchingStrategy>()
            where TMatchingStrategy : IMatchingStrategy<string>, new()
        {
            return (DataReaderAutoMap<TTarget>)base.Matching<TMatchingStrategy>();
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="sourceColumnName">Name of the source column to get</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <returns></returns>
        public new DataReaderAutoMap<TTarget> Specify(string sourceColumnName, Expression<Func<TTarget, object>> targetPropertyExpression)
        {
            return (DataReaderAutoMap<TTarget>)base.Specify(sourceColumnName, targetPropertyExpression);
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
        public new DataReaderAutoMap<TTarget> Specify<TInput, TOutput>(string sourceColumnName, Expression<Func<TTarget, TOutput>> targetPropertyExpression, Func<TInput, TOutput> conversion)
        {
            return (DataReaderAutoMap<TTarget>)base.Specify<TInput, TOutput>(sourceColumnName, targetPropertyExpression, conversion);
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="mappingAction">Action to populate target property value</param>
        /// <returns></returns>
        public new DataReaderAutoMap<TTarget> Specify(Action<IDataReader, TTarget> mappingAction)
        {
            return (DataReaderAutoMap<TTarget>)base.Specify(mappingAction);
        }

        /// <summary>
        /// Populate an existing target object from a source data reader, positioned at the correct row
        /// </summary>
        /// <remarks>
        /// Does not read from the data reader, so the reader needs to have been positioned first
        /// </remarks>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void PopulateTarget(IDataReader source, TTarget target)
        {
            var map = new DataReaderAutoMap<TTarget>();
            map.Populate(source, target);
        }

        /// <summary>
        /// Create a target object from a source data reader, positioned at the correct row
        /// </summary>
        /// <remarks>
        /// Does not read from the data reader, so the reader needs to have been positioned first
        /// </remarks>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget CreateTarget(IDataReader source)
        {
            var map = new DataReaderAutoMap<TTarget>();
            return map.Create(source);
        }

        /// <summary>
        /// Create a collection of target objects from a source data reader
        /// </summary>
        /// <remarks>
        /// Creates a target list and populates it from the source reader - reading from the reader for each 
        /// new target object. Can be called with a data reader which has not already been positioned
        /// </remarks>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TTarget> CreateTargetList(IDataReader source)
        {
            var map = new DataReaderAutoMap<TTarget>();
            var list = new List<TTarget>();
            while (source.Read())
            {
                list.Add(CreateTarget(source));
            }            
            return list;
        }
    }
}
