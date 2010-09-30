using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Sixeyed.Heartbeat.Entities;
using Sixeyed.Heartbeat.Extensions;
using Sixeyed.Heartbeat.Logging;

namespace Sixeyed.Heartbeat.Repositories
{
    /// <summary>
    /// Repository wrapping access to Service Status logs
    /// </summary>
    public static class HeartbeatRepository 
    {
        private static HeartbeatEntities GetContext()
        {
            var context = new HeartbeatEntities();
            context.HeartbeatLogs.MergeOption = MergeOption.NoTracking;
            return context;
        }

        /// <summary>
        /// Writes a new heartbeat log entry
        /// </summary>
        /// <param name="log">Log entry</param>
        public static void WriteHeartbeatLog(HeartbeatLog log)
        {
            try
            {
                using (var context = GetContext())
                {
                    context.AddToHeartbeatLogs(log);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error(() => "Exception writing heartbeat log: {0}, message: {1}"
                                    .FormatWith(log, ex.FullMessage()));
            }
        }

        /// <summary>
        /// Returns all heartbeat logs for a given instance
        /// </summary>
        /// <param name="heartbeatInstanceId"></param>
        public static IEnumerable<HeartbeatLog> GetHeartbeatLogs(Guid heartbeatInstanceId)
        {
            var logs = new List<HeartbeatLog>();
            using (var context = GetContext())
            {
                try
                {
                    logs.AddRange(from l in context.HeartbeatLogs
                                  where l.HeartbeatInstanceId == heartbeatInstanceId
                                  select l);
                }
                catch (Exception ex)
                {
                    Log.Error(() => "Exception loading heartbeat logs, heartbeatInstanceId: {0}, message: {1}"
                                        .FormatWith(heartbeatInstanceId, ex.FullMessage()));
                }
            }
            return logs;
        }
    }
}
