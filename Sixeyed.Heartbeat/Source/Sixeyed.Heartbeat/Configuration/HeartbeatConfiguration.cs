using System.Configuration;
using Sixeyed.Heartbeat.Logging;

namespace Sixeyed.Heartbeat.Configuration
{
    /// <summary>
    /// Section for configuring <see cref="Heartbeat"/>
    /// </summary>
    public class HeartbeatConfiguration : ConfigurationSection
    {
        private static bool _loggedWarning;

        /// <summary>
        /// Returns the currently configured settings
        /// </summary>
        public static HeartbeatConfiguration Current
        {
            get
            {
                var current = ConfigurationManager.GetSection("sixeyed.heartbeat") as HeartbeatConfiguration;
                if (current == null)
                {
                    current = new HeartbeatConfiguration();
                    if (!_loggedWarning)
                    {
                        Log.Warn("Configuration section: <sixeyed.heartbeat> not specified. Default configuration will be used");
                        _loggedWarning = true;
                    }
                }
                return current; 
            }
        }

        /// <summary>
        /// Returns whether the heartbeat is enabled
        /// </summary>
        /// <remarks>
        /// If not specified, defaults to true
        /// </remarks>
        [ConfigurationProperty(HeartbeatConfiguration.SettingName.Enabled, DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)this[SettingName.Enabled]; }
        }

        /// <summary>
        /// Returns the default interval in milliseconds for logging timed pulses
        /// </summary>
        /// <remarks>
        /// If not specified, defaults to 0 meaning timed pulses are not recorded
        /// </remarks>
        [ConfigurationProperty(HeartbeatConfiguration.SettingName.DefaultPulseTimerInterval, DefaultValue = DefaultValue.PulseTimerInterval)]
        public double DefaultPulseTimerInterval
        {
            get { return (double)this[SettingName.DefaultPulseTimerInterval]; }
        }

        /// <summary>
        /// Returns the default interval for logging counted pulses
        /// </summary>
        /// <remarks>
        /// If not specified, defaults to 0 meaning count pulses are not recorded
        /// </remarks>
        [ConfigurationProperty(HeartbeatConfiguration.SettingName.DefaultPulseCountInterval, DefaultValue = DefaultValue.PulseCountInterval)]
        public long DefaultPulseCountInterval
        {
            get { return (long)this[SettingName.DefaultPulseCountInterval]; }
        }

        /// <summary>
        /// Default values
        /// </summary>
        private struct DefaultValue
        {
            /// <summary>
            /// Timer interval = 0
            /// </summary>
            public const double PulseTimerInterval = 0;

            /// <summary>
            /// Count interval = 0
            /// </summary>
            public const long PulseCountInterval = 0;
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            /// <summary>
            /// defaultPulseTimerInterval
            /// </summary>
            public const string DefaultPulseTimerInterval = "defaultPulseTimerInterval";

            /// <summary>
            /// defaultPulseCountInterval
            /// </summary>
            public const string DefaultPulseCountInterval = "defaultPulseCountInterval";

            /// <summary>
            /// enabled
            /// </summary>
            public const string Enabled = "enabled";
        }
    }
}
