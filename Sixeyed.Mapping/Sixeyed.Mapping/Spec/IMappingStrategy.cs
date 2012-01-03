using System;
using System.Collections.Generic;

namespace Sixeyed.Mapping.Spec
{
    /// <summary>
    /// Represents a strategy for mapping from a source to a target
    /// </summary>
    /// <typeparam name="TMatchingLookup">Type of object used in matching target properties</typeparam>
    public interface IMappingStrategy<TMatchingLookup>
    {
        /// <summary>
        /// Gets all mappings between target and source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetType"></param>
        /// <param name="matching"></param>
        /// <param name="autoMapMissingTargets"></param>
        /// <returns></returns>
        List<IPropertyMapping> GetMappings(object source, Type targetType, IMatchingStrategy<TMatchingLookup> matching, bool autoMapMissingTargets);

        /// <summary>
        /// Set all mappings
        /// </summary>
        /// <param name="mappings"></param>
        void SetMappings(List<IPropertyMapping> mappings);

        /// <summary>
        /// Add a mapping
        /// </summary>
        /// <param name="mapping"></param>
        void AddMapping(IPropertyMapping mapping);
        
        /// <summary>
        /// Populate the target object from the source using the mappings
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        void Populate(object target, object source);

        /// <summary>
        /// Whether mappings have been generated
        /// </summary>
        bool GeneratedMappings { get; }

        /// <summary>
        /// Gets/sets the parent map
        /// </summary>
        IMap Map { get; set; }
    }
}
