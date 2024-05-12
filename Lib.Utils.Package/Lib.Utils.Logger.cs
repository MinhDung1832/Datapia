namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framework v4.5.2)

    using System;
    using System.IO;

    #endregion Using System Library (.NET Framework v4.5.2)

    public sealed class Logger
    {
        public enum LogType { INFO, WARN, ERROR }

        public static void Write(Exception ex, LogType type = LogType.ERROR, string folder = Constants.DefaultLogFolder, string file = Constants.DefaultLogFile)
        {
            Write(DateTime.Now.ToString(Constants.DateTimeFormat) + " - [" + Enum.GetName(typeof(LogType), type) + "]:" +
                  Environment.NewLine + " + Source: " + ex.Source.ToTrim() +
                  Environment.NewLine + " + Message: " + ex.Message.ToTrim(), folder, file);
        }

        public static void Write(string message, LogType type = LogType.ERROR, string folder = Constants.DefaultLogFolder, string file = Constants.DefaultLogFile)
        {
            Write(DateTime.Now.ToString(Constants.DateTimeFormat) + " - [" + Enum.GetName(typeof(LogType), type) + "]:" +
                  Environment.NewLine + " + Message: " + message.ToTrim(), folder, file);
        }

        static void Write(string message, string folder, string file)
        {
            try
            {
                using (TextWriter tw = File.AppendText(Path.Combine(folder, DateTime.Now.ToString(Constants.DateFormat) + "-" + file)))
                {
                    tw.WriteLine(message);
                    tw.Flush();
                    tw.Close();
                }
            }
            catch
            {
            }
        }
    }
}