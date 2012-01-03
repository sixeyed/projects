
namespace Sixeyed.Mapping
{
    /// <summary>
    /// Represents a map
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Gets/sets whether to throw exceptions if mapping fails
        /// </summary>
        bool ThrowMappingExceptions { get; set; }
    }
}
