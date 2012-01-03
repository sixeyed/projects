using System;
using System.Collections.Generic;
using System.Reflection;
using Sixeyed.Mapping.Spec;
using Sixeyed.Mapping.Exceptions;

namespace Sixeyed.Mapping.MappingStrategies
{
    /// <summary>
    /// Base implementation of <see cref="IMappingStrategy"/> - matching property names in the target from the source
    /// </summary>
    /// <typeparam name="TSource">Type of souce object</typeparam>
    /// <typeparam name="TSourceLookup">Type of target object</typeparam>
    /// <typeparam name="TMatchingLookup">Type of object used to matching target properties</typeparam>
    public abstract class MappingStrategy<TSource, TSourceLookup, TMatchingLookup> : IMappingStrategy<TMatchingLookup>
    {
        /// <summary>
        /// Returns whether mappings have been generated
        /// </summary>
        public bool GeneratedMappings { get; private set; }

        /// <summary>
        /// Gets/sets the owning map for the strategy
        /// </summary>
        public IMap Map { get; set; }

        private List<IPropertyMapping> _propertyMap;
        private List<string> _propertyMapLookup;

        private List<IPropertyMapping> PropertyMap
        {
            get
            {
                if (_propertyMap == null)
                {
                    _propertyMap = new List<IPropertyMapping>();
                }
                return _propertyMap;
            }
        
        }
        
        private List<string> PropertyMapLookup
        {
            get
            {
                if (_propertyMapLookup == null)
                {
                    _propertyMapLookup = new List<string>();
                }
                return _propertyMapLookup;
            }
        }

        /// <summary>
        /// Override in inheriting classes to create all the mappings for the target
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetType"></param>
        /// <param name="matching"></param>
        protected abstract void CreateMappings(TSource source, Type targetType, IMatchingStrategy<TMatchingLookup> matching);

        /// <summary>
        /// Override in inheriting classes to create a single mapping for the target
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        protected abstract IPropertyMapping CreateMapping(TSourceLookup source, PropertyInfo target);

        public void Populate(object target, object source)
        {
            foreach (IPropertyMapping mapping in PropertyMap)
            {
                OnBeforePopulate(mapping, target, source);
                DoMap(mapping, target, source);
            }
        }

        protected virtual void OnBeforePopulate(IPropertyMapping mapping, object target, object source)
        {
            //do nothing
        }

        private void DoMap(IPropertyMapping mapping, object target, object source)
        {
            if (mapping.CanRead(source) & mapping.CanWrite(target))
            {
                try
                {
                    mapping.MapValue(source, target);
                }
                catch (Exception ex)
                {
                    if (Map.ThrowMappingExceptions)
                    {
                        throw new MappingException(ex, "MappingStrategy.DoMap failed, target: {0}, error: {1}", mapping.TargetName, ex.Message);
                    }
                }
            }
        }

        protected virtual TSource GetSource(object sourceType)
        {
            return (TSource)sourceType;
        }

        /// <summary>
        /// Returns all the mappings for the target
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="targetType"></param>
        /// <param name="matching"></param>
        /// <param name="autoMapMissingTargets"></param>
        /// <returns></returns>
        public List<IPropertyMapping> GetMappings(object sourceType, Type targetType, IMatchingStrategy<TMatchingLookup> matching, bool autoMapMissingTargets)
        {
            if (autoMapMissingTargets && !GeneratedMappings)
            {
                CreateMappings(GetSource(sourceType), targetType, matching);
                GeneratedMappings = true;
            }
            return PropertyMap;
        }

        /// <summary>
        /// Sets mappings for the target
        /// </summary>
        /// <param name="mappings"></param>
        public void SetMappings(List<IPropertyMapping> mappings)
        {
            _propertyMap = mappings;
        }

        /// <summary>
        /// Adds a mapping for the target
        /// </summary>
        /// <param name="mapping"></param>
        public void AddMapping(IPropertyMapping mapping)
        {
            if (!PropertyMapLookup.Contains(mapping.TargetName))
            {
                PropertyMap.Add(mapping);
                PropertyMapLookup.Add(mapping.TargetName);
            }
        }
    }
}
