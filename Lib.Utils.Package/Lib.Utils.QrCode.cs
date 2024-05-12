namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framwork v4.5.2)

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using ZXing;
    using ZXing.Common;

    #endregion Using System Library (.NET Framwork v4.5.2)

    public enum Code2D
    {
        QR_CODE,
        DATA_MATRIX,
        AZTEC,
        PDF_417,
        MAXICODE
    }

    public enum Code1D
    {
        CODE_39,
        CODE_93,
        CODE_128,
        EAN_8,
        EAN_13,
        ITF,
        RSS_14
    }

    public sealed class QrcodeCommon
    {
        /// <summary>
        /// UTF-8 with the necessary ECI segment.
        /// </summary>
        public static bool UTF8 { private get; set; }

        /// <summary>
        /// UTF-8 with the ECI segment is omitted.
        /// </summary>
        public static bool UTF8_NO_ECI { private get; set; }

        /// <summary>
        /// Don't put the content string into the output image.
        /// </summary>
        public static bool PURGE_BARCODE { private get; set; }

        public static Bitmap EncodeToImage(string content, Code2D format = Code2D.QR_CODE, int width = 200, int height = 200, int margin = 0)
        {
            var writer = new BarcodeWriter
            {
                Format = format.To2DCodeFormat(),
                Options = new EncodingOptions
                {
                    Width = width,
                    Height = height,
                    Margin = margin,
                    PureBarcode = PURGE_BARCODE
                }
            };
            if (UTF8_NO_ECI)
            {
                writer.Options.Hints.Add(EncodeHintType.DISABLE_ECI, true);
                writer.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            }
            else if (UTF8)
            {
                writer.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            }
            return writer.Write(content);
        }

        public static byte[] EncodeToBytes(string content, Code2D format = Code2D.QR_CODE, int width = 200, int height = 200, int margin = 0)
        {
            var data = EncodeToImage(content, format, width, height, margin);
            return data?.ImageToBytes(ImageFormat.Bmp);
        }

        public static string EncodeToBase64String(string content, Code2D format = Code2D.QR_CODE, int width = 200, int height = 200, int margin = 0)
        {
            var data = EncodeToBytes(content, format, width, height, margin);
            return data?.ToBase64String();
        }

        public static string EncodeToBase64StringCss(string content, Code2D format = Code2D.QR_CODE, int width = 200, int height = 200, int margin = 0)
        {
            var data = EncodeToBase64String(content, format, width, height, margin);
            return data != null ? $"data:image/png;base64,{data}" : null;
        }

        public static string DecodeFromImage(Bitmap bitmap)
        {
            var source = new BitmapLuminanceSource(bitmap);
            var reader = new BarcodeReader(null, null, ls => new GlobalHistogramBinarizer(ls))
            {
                AutoRotate = true,
                TryInverted = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new List<BarcodeFormat>()
                    {
                        BarcodeFormat.QR_CODE,
                        BarcodeFormat.DATA_MATRIX,
                        BarcodeFormat.AZTEC,
                        BarcodeFormat.PDF_417,
                        BarcodeFormat.MAXICODE
                    }
                }
            };
            var result = reader.Decode(source);
            return result?.Text;
        }

        public static string DecodeMultipleFromImage(Bitmap bitmap)
        {
            var source = new BitmapLuminanceSource(bitmap);
            var reader = new BarcodeReader(null, null, ls => new GlobalHistogramBinarizer(ls))
            {
                AutoRotate = true,
                TryInverted = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new List<BarcodeFormat>()
                    {
                        BarcodeFormat.QR_CODE,
                        BarcodeFormat.DATA_MATRIX,
                        BarcodeFormat.AZTEC,
                        BarcodeFormat.PDF_417,
                        BarcodeFormat.MAXICODE
                    }
                }
            };
            var multiple = reader.DecodeMultiple(source);
            if (multiple.NotNull())
            {
                var result = string.Empty;
                var lastPosition = multiple.Length - 1;
                for (int i = 0; i <= lastPosition; i++)
                {
                    if (i != lastPosition)
                    {
                        result += multiple[i].NotNull() ? multiple[i].Text + Environment.NewLine : Environment.NewLine;
                    }
                    else
                    {
                        result += multiple[lastPosition].NotNull() ? multiple[lastPosition].Text : string.Empty;
                    }
                }
                return result;
            }
            return null;
        }

        public static string DecodeFromFile(string filename)
        {
            try { var bitmap = new Bitmap(filename); return DecodeFromImage(bitmap); }
            catch { return null; }
        }

        public static string DecodeMultipleFromFile(string filename)
        {
            try { var bitmap = new Bitmap(filename); return DecodeMultipleFromImage(bitmap); }
            catch { return null; }
        }

        public static string DecodeFromBase64String(string imgdata)
        {
            try { using (var ms = new MemoryStream(Convert.FromBase64String(imgdata))) { var bitmap = new Bitmap(ms); return DecodeFromImage(bitmap); } }
            catch { return null; }
        }

        public static string DecodeMultipleFromBase64String(string imgdata)
        {
            try { using (var ms = new MemoryStream(Convert.FromBase64String(imgdata))) { var bitmap = new Bitmap(ms); return DecodeMultipleFromImage(bitmap); } }
            catch { return null; }
        }
    }

    public sealed class BarcodeCommon
    {
        /// <summary>
        /// UTF-8 with the necessary ECI segment.
        /// </summary>
        public static bool UTF8 { private get; set; }

        /// <summary>
        /// UTF-8 with the ECI segment is omitted.
        /// </summary>
        public static bool UTF8_NO_ECI { private get; set; }

        /// <summary>
        /// Don't put the content string into the output image.
        /// </summary>
        public static bool PURGE_BARCODE { private get; set; }

        public static Bitmap EncodeToImage(string content, Code1D format = Code1D.CODE_128, int width = 215, int height = 86, int margin = 0)
        {
            var writer = new BarcodeWriter
            {
                Format = format.To1DCodeFormat(),
                Options = new EncodingOptions
                {
                    Width = width,
                    Height = height,
                    Margin = margin,
                    PureBarcode = PURGE_BARCODE
                }
            };
            if (UTF8_NO_ECI)
            {
                writer.Options.Hints.Add(EncodeHintType.DISABLE_ECI, true);
                writer.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            }
            else if (UTF8)
            {
                writer.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            }
            return writer.Write(content);
        }

        public static byte[] EncodeToBytes(string content, Code1D format = Code1D.CODE_128, int width = 215, int height = 86, int margin = 0)
        {
            var data = EncodeToImage(content, format, width, height, margin);
            return data?.ImageToBytes(ImageFormat.Bmp);
        }

        public static string EncodeToBase64String(string content, Code1D format = Code1D.CODE_128, int width = 215, int height = 86, int margin = 0)
        {
            var data = EncodeToBytes(content, format, width, height, margin);
            return data?.ToBase64String();
        }

        public static string EncodeToBase64StringCss(string content, Code1D format = Code1D.CODE_128, int width = 215, int height = 86, int margin = 0)
        {
            var data = EncodeToBase64String(content, format, width, height, margin);
            return data != null ? $"data:image/png;base64,{data}" : null;
        }

        public static string DecodeFromImage(Bitmap bitmap)
        {
            var source = new BitmapLuminanceSource(bitmap);
            var reader = new BarcodeReader(null, null, ls => new GlobalHistogramBinarizer(ls))
            {
                AutoRotate = true,
                TryInverted = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new List<BarcodeFormat>()
                    {
                        BarcodeFormat.CODE_39,
                        BarcodeFormat.CODE_93,
                        BarcodeFormat.CODE_128,
                        BarcodeFormat.EAN_8,
                        BarcodeFormat.EAN_13,
                        BarcodeFormat.ITF,
                        BarcodeFormat.RSS_14
                    }
                }
            };
            var result = reader.Decode(source);
            return result?.Text;
        }

        public static string DecodeMultipleFromImage(Bitmap bitmap)
        {
            var source = new BitmapLuminanceSource(bitmap);
            var reader = new BarcodeReader(null, null, ls => new GlobalHistogramBinarizer(ls))
            {
                AutoRotate = true,
                TryInverted = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new List<BarcodeFormat>()
                    {
                        BarcodeFormat.CODE_39,
                        BarcodeFormat.CODE_93,
                        BarcodeFormat.CODE_128,
                        BarcodeFormat.EAN_8,
                        BarcodeFormat.EAN_13,
                        BarcodeFormat.ITF,
                        BarcodeFormat.RSS_14
                    }
                }
            };
            var multiple = reader.DecodeMultiple(source);
            if (multiple.NotNull())
            {
                var result = string.Empty;
                var lastPosition = multiple.Length - 1;
                for (int i = 0; i <= lastPosition; i++)
                {
                    if (i != lastPosition)
                    {
                        result += multiple[i].NotNull() ? multiple[i].Text + Environment.NewLine : Environment.NewLine;
                    }
                    else
                    {
                        result += multiple[lastPosition].NotNull() ? multiple[lastPosition].Text : string.Empty;
                    }
                }
                return result;
            }
            return null;
        }

        public static string DecodeFromFile(string filename)
        {
            try { var bitmap = new Bitmap(filename); return DecodeFromImage(bitmap); }
            catch { return null; }
        }

        public static string DecodeMultipleFromFile(string filename)
        {
            try { var bitmap = new Bitmap(filename); return DecodeMultipleFromImage(bitmap); }
            catch { return null; }
        }

        public static string DecodeFromBase64String(string imgdata)
        {
            try { using (var ms = new MemoryStream(Convert.FromBase64String(imgdata))) { var bitmap = new Bitmap(ms); return DecodeFromImage(bitmap); } }
            catch { return null; }
        }

        public static string DecodeMultipleFromBase64String(string imgdata)
        {
            try { using (var ms = new MemoryStream(Convert.FromBase64String(imgdata))) { var bitmap = new Bitmap(ms); return DecodeMultipleFromImage(bitmap); } }
            catch { return null; }
        }
    }

    public static class BarcodeExtensions
    {
        public static BarcodeFormat To1DCodeFormat(this Code1D obj)
        {
            switch (obj)
            {
                case Code1D.CODE_39:
                    return BarcodeFormat.CODE_39;

                case Code1D.CODE_93:
                    return BarcodeFormat.CODE_93;

                case Code1D.CODE_128:
                    return BarcodeFormat.CODE_128;

                case Code1D.EAN_8:
                    return BarcodeFormat.EAN_8;

                case Code1D.EAN_13:
                    return BarcodeFormat.EAN_13;

                case Code1D.ITF:
                    return BarcodeFormat.ITF;

                default:
                    return BarcodeFormat.RSS_14;
            }
        }

        public static BarcodeFormat To2DCodeFormat(this Code2D obj)
        {
            switch (obj)
            {
                case Code2D.QR_CODE:
                    return BarcodeFormat.QR_CODE;

                case Code2D.DATA_MATRIX:
                    return BarcodeFormat.DATA_MATRIX;

                case Code2D.AZTEC:
                    return BarcodeFormat.AZTEC;

                case Code2D.PDF_417:
                    return BarcodeFormat.PDF_417;

                default:
                    return BarcodeFormat.MAXICODE;
            }
        }
    }
}