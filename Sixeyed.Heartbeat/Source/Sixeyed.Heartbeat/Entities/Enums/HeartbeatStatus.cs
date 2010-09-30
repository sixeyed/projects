using Sixeyed.Heartbeat.Attributes;

namespace Sixeyed.Heartbeat.Entities.Enums
{
    /// <summary>
    /// Represents the status of a <see cref="HeartbeatLog"/>
    /// </summary>
    public enum HeartbeatStatus
    {
        Null = 0,

        [DatabaseValue("START")]
        Started,

        [DatabaseValue("WORKING")]
        InProgress,

        [DatabaseValue("SUCCEED")]
        CompletedSucessfully,

        [DatabaseValue("FAIL")]
        CompletedButFailed,

        [DatabaseValue("UNKNOWN")]
        Unknown
    }
}
