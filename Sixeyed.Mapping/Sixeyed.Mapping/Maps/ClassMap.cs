using System;
using System.Linq.Expressions;
using Sixeyed.Mapping.MappingStrategies;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Base class for populating an object from another object 
    /// </summary>
    /// <remarks>
    /// By default, uses <see cref="SimpleNameMatchingStrategy"/>, matching target properties which have
    /// similar names to source properties. Ignores case and non-aplhanumeric characters
    /// </remarks>
    /// <typeparam name="TSource">Type of source object</typeparam>
    /// <typeparam name="TTarget">Type of target object</typeparam>
    public abstract class ClassMap<TSource, TTarget> : Map<TSource, TTarget, string>
        where TTarget : class, new()
        where TSource : class
    {
        protected override IMappingStrategy<string> GetDefaultMappingStrategy()
        {
            return new TypeMappingStrategy();
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
        public new ClassMap<TSource, TTarget> Cache<TCachingStrategy>()
            where TCachingStrategy : ICachingStrategy, new()
        {
            return (ClassMap<TSource, TTarget>)base.Cache<TCachingStrategy>();
        }

        /// <summary>
        /// Specify the matching strategy for the map, used to match source and target properties
        /// </summary>
        /// <typeparam name="TMatchingStrategy"></typeparam>
        /// <returns></returns>
        public new ClassMap<TSource, TTarget> Matching<TMatchingStrategy>()
            where TMatchingStrategy : IMatchingStrategy<string>, new()
        {
            return (ClassMap<TSource, TTarget>)base.Matching<TMatchingStrategy>();
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <typeparam name="TInput">Source property type</typeparam>
        /// <typeparam name="TOutput">Target property type</typeparam>
        /// <param name="sourcePropertyExpression">Accessor for the source property to get</param>
        /// <param name="targetPropertyExpression">Accessor for the target property to set</param>
        /// <returns></returns>
        public ClassMap<TSource, TTarget> Specify<TInput, TOutput>(Func<TSource, TInput> sourcePropertyExpression, Expression<Func<TTarget, TOutput>> targetPropertyExpression)
        {
            var mapping = new FuncMapping<TSource, TInput, TTarget, TOutput>(sourcePropertyExpression, targetPropertyExpression);
            MappingStrategy.AddMapping(mapping);
            return this;
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
        public ClassMap<TSource, TTarget> Specify<TInput, TOutput>(Func<TSource, TInput> sourcePropertyExpression, Expression<Func<TTarget, TOutput>> targetPropertyExpression, Func<TInput, TOutput> conversion)
        {
            var mapping = new FuncMapping<TSource, TInput, TTarget, TOutput>(sourcePropertyExpression, targetPropertyExpression);
            mapping.SetConversion<TInput, TOutput>(conversion);
            MappingStrategy.AddMapping(mapping);
            return this;
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="mappingAction">Action to populate target property value</param>
        /// <returns></returns>
        public new ClassMap<TSource, TTarget> Specify(Action<TSource, TTarget> mappingAction)
        {
            return (ClassMap<TSource, TTarget>)base.Specify(mappingAction);
        }
    }
}
