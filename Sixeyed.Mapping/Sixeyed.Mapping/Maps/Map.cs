using System;
using System.Collections.Generic;
using System.Text;
using Sixeyed.Mapping.CachingStrategies;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Base class for all maps
    /// </summary>
    /// <typeparam name="TSource">Type of source object</typeparam>
    /// <typeparam name="TTarget">Type of target object</typeparam>
    /// <typeparam name="TMatchingLookup">Type of object used to match source fields</typeparam>
    public abstract class Map<TSource, TTarget, TMatchingLookup> : IMap
        where TTarget : class, new()
    {
        private IMappingStrategy<TMatchingLookup> _mappingStrategy;
        private IMatchingStrategy<TMatchingLookup> _matchingStrategy;
        private ICachingStrategy _cachingStrategy { get; set; }

        /// <summary>
        /// Gets the mapping strategy, used to read source values and set target values
        /// </summary>
        protected IMappingStrategy<TMatchingLookup> MappingStrategy 
        {
            get 
            {
                if (_mappingStrategy == null)
                {
                    _mappingStrategy = GetDefaultMappingStrategy();
                    _mappingStrategy.Map = this;
                }
                return _mappingStrategy;
            }            
        }       
        
        /// <summary>
        /// Gets/sets the matching strategy, used to match source and target properties
        /// </summary>
        protected IMatchingStrategy<TMatchingLookup> MatchingStrategy 
        {
            get
            {
                if (_matchingStrategy == null)
                {
                    _matchingStrategy = GetDefaultMatchingStrategy();
                }
                return _matchingStrategy;
            }
            set { _matchingStrategy = value; }
        }

        /// <summary>
        /// Gets/sets the caching strategy, used to cache the property match between source and target
        /// </summary>
        protected ICachingStrategy CachingStrategy
        {
            get
            {
                if (_cachingStrategy == null)
                {
                    _cachingStrategy = new NullCachingStrategy();
                }
                return _cachingStrategy;
            }
            set { _cachingStrategy = value; }
        }

        /// <summary>
        /// Whether to automatically matach & map unspecified target properties
        /// </summary>
        public virtual bool AutoMapUnspecifiedTargets { get; set; }

        /// <summary>
        /// Whether to throw <see cref="MappingException"/> if unable to map properties
        /// </summary>
        public bool ThrowMappingExceptions { get; set; }

        /// <summary>
        /// Gets the default mapping strategy for the map
        /// </summary>
        /// <returns></returns>
        protected abstract IMappingStrategy<TMatchingLookup> GetDefaultMappingStrategy();

        /// <summary>
        /// Gets the default matching strategy for the map
        /// </summary>
        /// <returns></returns>
        protected abstract IMatchingStrategy<TMatchingLookup> GetDefaultMatchingStrategy();               

        /// <summary>
        /// Specify the caching strategy for the map, used to cache the property match between source and target
        /// </summary>
        /// <typeparam name="TCachingStrategy"></typeparam>
        /// <returns></returns>
        public Map<TSource, TTarget, TMatchingLookup> Cache<TCachingStrategy>()
            where TCachingStrategy : ICachingStrategy, new()
        {
            CachingStrategy = new TCachingStrategy();
            return this;
        }

        /// <summary>
        /// Specify the matching strategy for the map, used to match source and target properties
        /// </summary>
        /// <typeparam name="TMatchingStrategy"></typeparam>
        /// <returns></returns>
        public Map<TSource, TTarget, TMatchingLookup> Matching<TMatchingStrategy>()
            where TMatchingStrategy : IMatchingStrategy<TMatchingLookup>, new()
        {
            MatchingStrategy = new TMatchingStrategy();
            return this;
        }

        /// <summary>
        /// Specify an explicit property mapping
        /// </summary>
        /// <param name="mappingAction">Action to populate target property value</param>
        /// <returns></returns>
        protected internal Map<TSource, TTarget, TMatchingLookup> Specify(Action<TSource, TTarget> mappingAction)
        {
            var mapping = new ActionMapping<TSource, TTarget>(mappingAction);
            MappingStrategy.AddMapping(mapping);
            return this;
        }

        /// <summary>
        /// Clear and populate a collection of target objects from a source collection
        /// </summary>
        /// <remarks>
        /// Clears the target list and then adds a mapped target object for each source item
        /// </remarks>
        /// <param name="targetList"></param>
        /// <param name="sourceList"></param>
        public void PopulateList(IList<TTarget> targetList, IList<TSource> sourceList)
        {           
            targetList.Clear();
            AppendList(targetList, sourceList);
        }

        /// <summary>
        /// Clear and populate a collection of target objects from a source array
        /// </summary>
        /// <remarks>
        /// Clears the target list and then adds a mapped target object for each source item
        /// </remarks>
        /// <param name="targetList"></param>
        /// <param name="sourceArray"></param>
        public void PopulateList(IList<TTarget> targetList, params TSource[] sourceArray)
        {
            PopulateList(targetList, new List<TSource>(sourceArray));
        }

        /// <summary>
        /// Append a collection of target objects from a source collection
        /// </summary>
        /// <remarks>
        /// Adds a mapped target object for each source item, without clearing the target list
        /// </remarks>
        /// <param name="targetList"></param>
        /// <param name="sourceList"></param>
        public void AppendList(IList<TTarget> targetList, IList<TSource> sourceList)
        {
            var newList = CreateList(sourceList);
            foreach (TTarget target in newList)
            {
                targetList.Add(target);
            }
        }

        /// <summary>
        /// Append a collection of target objects from a source array
        /// </summary>
        /// <remarks>
        /// Adds a mapped target object for each source item, without clearing the target list
        /// </remarks>
        /// <param name="targetList"></param>
        /// <param name="sourceArray"></param>
        public void AppendList(IList<TTarget> targetList, params TSource[] sourceArray)
        {
            AppendList(targetList, new List<TSource>(sourceArray));
        }

        /// <summary>
        /// Create a collection of target objects from a source array
        /// </summary>
        /// <param name="sourceArray"></param>
        /// <returns></returns>
        public virtual List<TTarget> CreateList(params TSource[] sourceArray)
        {
            var sourceList = new List<TSource>(sourceArray);
            return CreateList(sourceList);
        }

        /// <summary>
        /// Create a collection of target objects from a source collection
        /// </summary>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public virtual List<TTarget> CreateList(IList<TSource> sourceList)
        {
            var targetList = new List<TTarget>(sourceList.Count);
            foreach (TSource source in sourceList)
            {
                targetList.Add(Create(source));
            }
            return targetList;
        }

        /// <summary>
        /// Before populating a target object, generates mappings or loads from cache
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        protected virtual void OnBeforePopulate(TTarget target, TSource source)
        {
            if (CachingStrategy is NullCachingStrategy && !MappingStrategy.GeneratedMappings)
            {
                MappingStrategy.GetMappings(source, typeof(TTarget), MatchingStrategy, AutoMapUnspecifiedTargets);
            }
            else
            {
                var cacheKey = GetCacheKey();
                var cachedMappings = CachingStrategy.Get(cacheKey) as List<IPropertyMapping>;
                if (cachedMappings == null)
                {
                    cachedMappings = MappingStrategy.GetMappings(source, typeof(TTarget), MatchingStrategy, AutoMapUnspecifiedTargets);
                    CachingStrategy.Set(cacheKey, cachedMappings);
                }
                else
                {
                    MappingStrategy.SetMappings(cachedMappings);
                }
            }
        }

        /// <summary>
        /// Populate an existing target object from source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void Populate(TSource source, TTarget target)
        {
            OnBeforePopulate(target, source);
            MappingStrategy.Populate(target, source);
        }

        /// <summary>
        /// Create a target object from source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public TTarget Create(TSource source)
        {
            var target = new TTarget();
            Populate(source, target);
            return target;
        }

        private string GetCacheKey()
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.AppendFormat("{0}_", GetType().FullName);
            keyBuilder.AppendFormat("{0}_", typeof(TTarget).FullName);
            keyBuilder.AppendFormat("{0}_", typeof(TSource).FullName);
            keyBuilder.AppendFormat("{0}", MatchingStrategy.GetType().FullName);
            return keyBuilder.ToString();
        }
    }
}
