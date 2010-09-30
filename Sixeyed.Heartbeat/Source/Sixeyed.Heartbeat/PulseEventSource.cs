
namespace Sixeyed.Heartbeat
{
    /// <summary>
    /// Represents the source of a pulse event
    /// </summary>
    public enum PulseEventSource
    {
        /// <summary>
        /// Heartbeat started
        /// </summary>
        Start,

        /// <summary>
        /// Pulse fired from count increment
        /// </summary>
        Count, 

        /// <summary>
        /// Pulse fired from timer inerval
        /// </summary>
        Timer,

        /// <summary>
        /// Heartbeat ended
        /// </summary>
        End
    }
}
