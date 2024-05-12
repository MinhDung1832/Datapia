namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framework v4.5.2)

    using System.Diagnostics;

    #endregion Using System Library (.NET Framework v4.5.2)

    public sealed class LogTrace
    {
        public enum EventId
        {
            Base = 1000,
            Layout = 1001,
            Control = 1002,
            Function = 1003,
            Connection = 1004,
            Inquiry = 1005,
            Insert = 1006,
            Update = 1007,
            Delete = 1008,
            Execute = 1009,
            Convert = 1010,
            Unknown = 1011
        }

        private readonly string mySource;
        private readonly string myNewLog;

        public LogTrace(string source = null)
        {
            if (source.IsNull())
                source = "Application";
            this.mySource = source;
            this.myNewLog = source;
        }

        public void Write(string eventMessage, EventId eventId = EventId.Base, EventLogEntryType eventType = EventLogEntryType.Error)
        {
            try
            {
                if (!EventLog.SourceExists(mySource))
                {
                    EventLog.CreateEventSource(mySource, myNewLog);
                    return;
                }
                EventLog myLog = new EventLog
                {
                    Source = mySource
                };
                myLog.WriteEntry(eventMessage, eventType, (int)eventId);
            }
            catch
            {
            }
        }
    }
}