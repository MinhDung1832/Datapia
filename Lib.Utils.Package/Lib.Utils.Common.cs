namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framework v4.5.2)

    using System;
    using System.IO;

    #endregion Using System Library (.NET Framework v4.5.2)

    public class LupCommon
    {
        public static bool IsDateTime(string date, string format, out DateTime result) => DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out result);

        public static bool IsDateTime(string date, string[] formats, out DateTime result) => DateTime.TryParseExact(date, formats, null, System.Globalization.DateTimeStyles.None, out result);

        public static string GetMimeTypeByFileName(string fileName)
        {
            var mime = "application/octet-stream";
            var extension = Path.GetExtension(fileName);
            if (extension.NotNull())
            {
                extension = extension.Replace(".", "");
                extension = extension.ToLower();
                switch (extension)
                {
                    case "xls":
                    case "xlsx":
                        mime = "application/ms-excel";
                        break;

                    case "doc":
                    case "docx":
                        mime = "application/msword";
                        break;

                    case "ppt":
                    case "pptx":
                        mime = "application/ms-powerpoint";
                        break;

                    case "rtf":
                        mime = "application/rtf";
                        break;

                    case "zip":
                        mime = "application/zip";
                        break;

                    case "mp3":
                        mime = "audio/mpeg";
                        break;

                    case "bmp":
                        mime = "image/bmp";
                        break;

                    case "gif":
                        mime = "image/gif";
                        break;

                    case "jpg":
                    case "jpeg":
                        mime = "image/jpeg";
                        break;

                    case "png":
                        mime = "image/png";
                        break;

                    case "tiff":
                    case "tif":
                        mime = "image/tiff";
                        break;

                    case "txt":
                        mime = "text/plain";
                        break;
                }
            }
            return mime;
        }
    }
}