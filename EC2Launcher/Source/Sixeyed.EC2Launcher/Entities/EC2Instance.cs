using Sixeyed.EC2Launcher.Structs;

namespace Sixeyed.EC2Launcher.Entities
{
    /// <summary>
    /// Represents an EC2 image instance
    /// </summary>
    public class EC2Instance
    {
        /// <summary>
        /// Gets/sets the instance ID
        /// </summary>
        public string InstanceId { get; set; }

        /// <summary>
        /// Gets/sets the name of the current instance state
        /// </summary>
        public string CurrentStateName { get; set; }

        /// <summary>
        /// Gets/sets the public DNS name for the instance
        /// </summary>
        public string PublicDnsName { get; set; }

        /// <summary>
        /// Returns whether the instance is currently running
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            return CurrentStateName == EC2InstanceState.Running;
        }
    }
}
