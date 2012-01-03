using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sixeyed.Mapping.MappingStrategies;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Automap for populating an object from another object
    /// </summary>
    /// <remarks>
    /// By default, uses <see cref="SimpleNameMatchingStrategy"/>, matching target properties which have
    /// similar names to source properties. Ignores case and non-aplhanumeric characters
    /// </remarks>
    /// <typeparam name="TSource">Type of source object</typeparam>
    /// <typeparam name="TTarget">Type of target object</typeparam>
    public class AutoMap<TSource, TTarget> : ClassMap<TSource, TTarget>
        where TTarget : class, new()
        where TSource : class
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
        public new AutoMap<TSource, TTarget> Cache<TCachingStrategy>()
            where TCachingStrategy : ICachingStrategy, new()
        {
            return (AutoMap<TSource, TTarget>)base.Cache<TCachingStrategy>();
        }

        /// <summary>
        /// Specify the matching strategy for the map, used to match source and target properties
        /// </summary>
        /// <typeparam name="TMatchingStrategy"></typeparam>
        /// <returns></returns>
        public new AutoMap<TSource, TTarget> Matching<TMatchingStrategy>()
            where TMatchingStrategy : IMatchingStrategy<string>, new()
        {
            return (AutoMap<TSource, TTarget>)base.Matching<TMatchingStrategy>();
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <typeparam name="TInput">Source property type</typeparam>
        /// <typeparam name="TOutput">Target property type</typeparam>
        /// <param name="sourcePropertyExpression">Accessor for the source property to get</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <returns></returns>
        public new AutoMap<TSource, TTarget> Specify<TInput, TOutput>(Func<TSource, TInput> sourcePropertyExpression, Expression<Func<TTarget, TOutput>> targetPropertyExpression)
        {
            return (AutoMap<TSource, TTarget>)base.Specify<TInput, TOutput>(sourcePropertyExpression, targetPropertyExpression);
        }

        /// <summary>
        /// Specify an explicit property mapping with a value conversion
        /// </summary>
        /// <typeparam name="TInput">Source property type</typeparam>
        /// <typeparam name="TOutput">Target property type</typeparam>
        /// <param name="sourcePropertyExpression">Accessor for the source property to get</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <param name="conversion">Conversion function to apply to the source value</param>
        /// <returns></returns>
        public new AutoMap<TSource, TTarget> Specify<TInput, TOutput>(Func<TSource, TInput> sourcePropertyExpression, Expression<Func<TTarget, TOutput>> targetPropertyExpression, Func<TInput, TOutput> conversion)
        {
            return (AutoMap<TSource, TTarget>)base.Specify<TInput, TOutput>(sourcePropertyExpression, targetPropertyExpression, conversion);
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="mappingAction">Action to populate target property value</param>
        /// <returns></returns>
        public new AutoMap<TSource, TTarget> Specify(Action<TSource, TTarget> mappingAction)
        {
            return (AutoMap<TSource, TTarget>)base.Specify(mappingAction);
        }

        /// <summary>
        /// Populate an existing target object from a source object
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void PopulateTarget(TSource source, TTarget target)
        {
            var map = new AutoMap<TSource, TTarget>();
            map.Populate(source, target);
        }

        /// <summary>
        /// Create a target object populated from a source object
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget CreateTarget(TSource source)
        {
            var map = new AutoMap<TSource, TTarget>();
            return map.Create(source);
        }

        /// <summary>
        /// Create a collection of target objects from a collection of source objects
        /// </summary>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public static List<TTarget> CreateTargetList(IList<TSource> sourceList)
        {
            var map = new AutoMap<TSource, TTarget>();
            return map.CreateList(sourceList);
        }
    }
}
