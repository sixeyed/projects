using Sixeyed.Heartbeat.Entities.Enums;
using Sixeyed.Heartbeat.Extensions;

namespace Sixeyed.Heartbeat.Entities
{
    /// <summary>
    /// Represents the logging of a heartbeat event
    /// </summary>
    public partial class HeartbeatLog
    {
        /// <summary>
        /// Gets/sets the status for this event
        /// </summary>
        public HeartbeatStatus Status
        {
            get { return EnumExtensions.FromDatabaseValue<HeartbeatStatus>(StatusCode); }
            set { StatusCode = value.ToDatabaseValue(); }
        }

        /// <summary>
        /// Returns a formatted string representing the log
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "HeartbeatLog[HeartbeatInstanceId: {0}, ComponentTypeName: {1}, StatusCode: {2}"
                    .FormatWith(HeartbeatInstanceId, ComponentTypeName, StatusCode);
        }
    }
}
