using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;
using System.Diagnostics;

namespace DVLD_Shared
{
    /// <summary>
    /// Provides static methods to write exception details and informational messages to the Windows Event Log.
    /// </summary>
    /// <remarks>Initializes a dedicated event source ('DVLD_Application') in the Application log if it does
    /// not exist. Logging operations swallow exceptions to avoid throwing from logging paths. Exception logging records
    /// the exception message and stack trace with optional custom text. Note: the current LogInfo implementation writes
    /// entries using EventLogEntryType.Error.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_Shared)]
    [DocInfo("Logging Utility", Version = "1.0")]
    public static class clsEventLogger
    {
        /// <summary>
        /// Application event source name used for logging.
        /// </summary>
        private const string SourceName = "DVLD_Application";

        /// <summary>
        /// Name of the Windows Application event log used for logging.
        /// </summary>
        /// <remarks>Represents the event log target used when writing event log entries.</remarks>
        private const string LogName = "Application";

        /// <summary>
        /// Initializes the clsEventLogger type and ensures the configured Windows Event Log source and log exist by
        /// creating the source if it does not exist.
        /// </summary>
        /// <remarks>Exceptions thrown while checking or creating the event source are caught and
        /// suppressed to avoid type initialization failures. Creating an event source can require administrative
        /// privileges and may be slow; consider creating the source during installation instead.</remarks>
        static clsEventLogger()
        {
            try
            {
                if(!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName); 
                }
            }
            catch
            {
                 
            }
        }

        /// <summary>
        /// Logs the provided exception and an optional custom message to the Windows event log as an error.
        /// </summary>
        /// <remarks>Writes the combined message to the event log via EventLog.WriteEntry with SourceName
        /// and EventLogEntryType.Error. Exceptions thrown while writing the event are swallowed. Ensure the event
        /// source exists and the process has permission to write to the event log.</remarks>
        /// <param name="ex">Exception to log.</param>
        /// <param name="customMessage">Optional additional details to include in the log entry; ignored when empty.</param>
        public static void LogException(Exception ex, string customMessage = "")
        {
            StringBuilder sb = new StringBuilder();
            
            if(!string.IsNullOrEmpty(customMessage))
            {
                sb.AppendLine($"Details: {customMessage}"); 
            }

            sb.AppendLine($"Exception Type: {ex.GetType().FullName}");

            sb.AppendLine($"Exception Message: {ex.Message}");

            sb.AppendLine($"Stack Trace: {ex.StackTrace}"); 

            if(ex.InnerException != null)
            {
                sb.AppendLine($"Inner Exception : {ex.InnerException.Message}");
            }

            try
            {
                EventLog.WriteEntry(SourceName, sb.ToString(), EventLogEntryType.Error); 
            }
            catch
            {

            }
        }

        /// <summary>
        /// Writes the specified message to the Windows event log using the configured SourceName as an error entry.
        /// </summary>
        /// <remarks>Uses EventLog.WriteEntry with EventLogEntryType.Error. SourceName must be registered
        /// and the caller requires permissions to write to the event log; EventLog.WriteEntry may throw
        /// exceptions.</remarks>
        /// <param name="message">The message to write to the event log.</param>
        public static void LogInfo(string message)
        {
            EventLog.WriteEntry(SourceName, message, EventLogEntryType.Error); 
        }
    }
}
