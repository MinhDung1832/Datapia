namespace Lib.Utils.Package
{
    internal class Constants
    {
        public const string DefaultLogFile = "Logs.txt";
        public const string DefaultLogFolder = ".\\";
        public const string Time12Format = "h:mm:ss tt";
        public const string Time24Format = "H:mm:ss";
        public const string DateFormat = "yyyyMMdd";
        public const string DateTimeFormat = "yyyy-MM-dd H:mm:ss";
        public const string SqlDateFormat = "yyyy-MM-dd";
        public const string SqlDateTimeFormat = "yyyy-MM-dd H:mm:ss";
        public const string DefaultPrintFromCell = "A1";
        public const string DefaultSheetName = "Sheet1";
        public const string DefaultFontName = "Times New Roman";
        public const float DefaultFontSize = 11.5f;
        public static readonly string[] NullDate = { "01/01/0001", "01/01/1753", "31/12/9998", "31/12/9999" };
        public static readonly string[] ExcelDefaultDateForTime = { "12/30/1899", "30/12/1899", "1899-12-30" };
        public static readonly string[] PopularDateFormats = {
            // Vietnam date formats
            "dd/MM/yyyy", "dd/MM/yy",
            "dd-MM-yyyy", "dd-MM-yy",
            // Windows date formats
            "M/d/yyyy","M/d/yy",
            "MM/dd/yy", "MM/dd/yyyy",
            "yy/MM/dd", "yyyy-MM-dd",
            "dd-MMM-yy", "dd-MMM-yyyy",
            // Number date formats
            "yyyyMMdd", "ddMMyyyy"
        };
        public static readonly string[] PopularDateTimeFormats = {
            // Vietnam datetime formats (12h)
            "dd/MM/yyyy h:mm:ss tt", "dd/MM/yy h:mm:ss tt",
            "dd-MM-yyyy h:mm:ss tt", "dd-MM-yy h:mm:ss tt",
            // Vietnam datetime formats (24h)
            "dd/MM/yyyy H:mm:ss", "dd/MM/yy H:mm:ss",
            "dd-MM-yyyy H:mm:ss", "dd-MM-yy H:mm:ss",
            // Windows datetime formats (12h)
            "M/d/yyyy h:mm:ss tt","M/d/yy h:mm:ss tt",
            "MM/dd/yy h:mm:ss tt", "MM/dd/yyyy h:mm:ss tt",
            "yy/MM/dd h:mm:ss tt", "yyyy-MM-dd h:mm:ss tt",
            "dd-MMM-yy h:mm:ss tt", "dd-MMM-yyyy h:mm:ss tt",
            // Windows datetime formats (24h)
            "M/d/yyyy H:mm:ss","M/d/yy H:mm:ss",
            "MM/dd/yy H:mm:ss", "MM/dd/yyyy H:mm:ss",
            "yy/MM/dd H:mm:ss", "yyyy-MM-dd H:mm:ss",
            "dd-MMM-yy H:mm:ss", "dd-MMM-yyyy H:mm:ss"
        };
    }
}