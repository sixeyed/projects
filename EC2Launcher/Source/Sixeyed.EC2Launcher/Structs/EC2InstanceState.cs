
namespace Sixeyed.EC2Launcher.Structs
{
    /// <summary>
    /// Known states for EC2 instances
    /// </summary>
    public struct EC2InstanceState
    {
        /// <summary>
        /// stopped
        /// </summary>
        public const string Stopped = "stopped";

        /// <summary>
        /// stopping
        /// </summary>
        public const string Stopping = "stopping";

        /// <summary>
        /// running
        /// </summary>
        public const string Running = "running";

        /// <summary>
        /// pending
        /// </summary>
        public const string Pending = "pending";
    }
}
