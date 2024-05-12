namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framework v4.5.2)

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    #endregion Using System Library (.NET Framework v4.5.2)

    public enum SplitMode { Normal = 1, Rigid = 2 }

    public enum DisplayMode { None = 1, MinValue = 2, MaxValue = 3 }

    public static class Extensions
    {
        #region Base Extensions

        public static string ToBase64String(this byte[] source) => source != null ? Convert.ToBase64String(source) : null;

        public static string ToBase64String(this byte[] source, int offset, int length) => source != null && offset >= 0 && length >= 0 && offset + length <= source.Length ? Convert.ToBase64String(source, offset, length) : null;

        public static byte[] FromBase64String(this string source) => source != null ? Convert.FromBase64String(source) : null;

        public static string UTF8GetString(this byte[] source) => source != null ? Encoding.UTF8.GetString(source) : null;

        public static byte[] UTF8GetBytes(this string source) => source != null ? Encoding.UTF8.GetBytes(source) : null;

        public static string ToTrim(this string source) => source != null ? source.Trim() : "";

        public static string ToTrim(this object source) => source != null ? source.ToString().Trim() : "";

        public static string[] Split(this string source, string separator, SplitMode option = SplitMode.Normal)
        {
            return source == null
                    ? option == SplitMode.Normal
                    ? new string[] { "" }
                    : new string[] { }
                    : separator == null
                    ? option == SplitMode.Normal
                    ? new string[] { source }
                    : new string[] { }
                    : option == SplitMode.Normal || (option == SplitMode.Rigid && source.Contains(separator))
                    ? source.Split(new string[] { separator }, StringSplitOptions.None)
                    : new string[] { };
        }

        public static string ToTitleCase(this string source) => source != null ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source.ToLower()) : "";

        public static string ToPascalCase(this string source)
        {
            if (source == null) return "";
            if (source.Length <= 1) return source.ToUpper();
            var words = source.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            var result = "";
            foreach (var word in words)
                result += $"{word.Substring(0, 1).ToUpper()}{word.Substring(1).ToLower()}";
            return result;
        }

        public static string ToCamelCase(this string source)
        {
            if (source == null) return "";
            if (source.Length <= 1) return source;
            var words = source.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            var result = words[0].ToLower();
            for (var i = 1; i < words.Length; i++)
                result += $"{words[i].Substring(0, 1).ToUpper()}{words[i].Substring(1).ToLower()}";
            return result;
        }

        public static string ToProperCase(this string source)
        {
            if (source == null) return "";
            if (source.Length <= 1) return source.ToUpper();
            var result = source.Substring(0, 1).ToUpper();
            for (var i = 1; i < source.Length; i++)
            {
                if (char.IsUpper(source[i])) result += " ";
                result += source[i];
            }
            return result;
        }

        #endregion Base Extensions

        #region Utility Extensions

        public static string UpperCase(this string source) => source.ToTrim().ToUpper();

        public static string LowerCase(this string source) => source.ToTrim().ToLower();

        public static string UpperCase(this object source) => source.ToTrim().UpperCase();

        public static string LowerCase(this object source) => source.ToTrim().LowerCase();

        public static bool IsNull<T>(this T source) where T : class => typeof(T) == typeof(string) ? string.IsNullOrWhiteSpace(source as string) : (source == null);

        public static bool NotNull<T>(this T source) where T : class => !source.IsNull();

        public static bool HasItem<T>(this T[] source) => source.NotNull() && source.Length > 0;

        public static bool HasItem<T>(this T[] source, int count) => source.NotNull() && source.Length == count;

        public static bool HasItem<T>(this List<T> source) => source.NotNull() && source.Count > 0;

        public static bool HasItem<T>(this List<T> source, int count) => source.NotNull() && source.Count == count;

        public static bool HasItem<T>(this IList<T> source) => source.NotNull() && source.Count > 0;

        public static bool HasItem<T>(this IList<T> source, int count) => source.NotNull() && source.Count == count;

        public static bool HasItem<T>(this IEnumerable<T> source) => source.NotNull() && source.Count() > 0;

        public static bool HasItem<T>(this IEnumerable<T> source, int count) => source.NotNull() && source.Count() == count;

        public static bool HasItem<TKey, TValue>(this Dictionary<TKey, TValue> source) => source.NotNull() && source.Count > 0;

        public static bool HasItem<TKey, TValue>(this Dictionary<TKey, TValue> source, int count) => source.NotNull() && source.Count == count;

        public static bool HasItem<TKey, TValue>(this SpecialDict<TKey, TValue> source) => source.NotNull() && source.HasVal;

        public static bool HasItem<TKey, TValue>(this SpecialDict<TKey, TValue> source, int count) => source.NotNull() && source.Count == count;

        public static bool HasItem(this DataSet source) => source.NotNull() && source.Tables.Count > 0;

        public static bool HasItem(this DataSet source, int count) => source.NotNull() && source.Tables.Count == count;

        public static bool HasRow(this DataTable source) => source.NotNull() && source.Rows.Count > 0;

        public static bool HasRow(this DataTable source, int count) => source.NotNull() && source.Rows.Count == count;

        public static bool HasCol(this DataTable source, string name) => source.NotNull() && source.Columns.Contains(name);

        public static bool HasCol(this DataTable source, int count) => source.NotNull() && source.Columns.Count == count;

        public static bool IsNumeric(this string source) => Regex.IsMatch(source.ToTrim().Replace(".", ""), @"^-?(0|[1-9]\d*)$");

        public static bool IsNumeric(this string source, out decimal outnum)
        {
            bool match = Regex.IsMatch(source.ToTrim().Replace(".", ""), @"^-?(0|[1-9]\d*)$");
            outnum = match ? source.ToDecimal(default(decimal)).Value : default;
            return match;
        }

        public static bool IsInteger(this string source) => Regex.IsMatch(source.ToTrim(), @"^([+-]?[1-9]\d*|0)$");

        public static bool IsInteger(this string source, out long outnum)
        {
            bool match = Regex.IsMatch(source.ToTrim(), @"^([+-]?[1-9]\d*|0)$");
            outnum = match ? source.ToLong(default(long)).Value : default;
            return match;
        }

        public static bool IsInteger(this string source, out int outnum)
        {
            bool match = Regex.IsMatch(source.ToTrim(), @"^([+-]?[1-9]\d*|0)$");
            outnum = match ? source.ToInt(default(int)).Value : default;
            return match;
        }

        public static bool IsInteger(this string source, out short outnum)
        {
            bool match = Regex.IsMatch(source.ToTrim(), @"^([+-]?[1-9]\d*|0)$");
            outnum = match ? source.ToShort(default(short)).Value : default;
            return match;
        }

        public static bool IsInteger(this string source, out byte outnum)
        {
            bool match = Regex.IsMatch(source.ToTrim(), @"^([+-]?[1-9]\d*|0)$");
            outnum = match ? source.ToByte(default(byte)).Value : default;
            return match;
        }

        #endregion Utility Extensions

        #region Convert Extensions

        public static double ToJulianDate(this DateTime source)
        {
            try
            {
                return source.ToOADate() + 2415018.5;
            }
            catch
            {
                return 0.0;
            }
        }

        public static DateTime ToDateTime(this double source)
        {
            try
            {
                return DateTime.FromOADate(source - 2415018.5);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime ToDateTime(this string source)
        {
            try
            {
                return DateTime.Parse(source.Trim());
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime ToDateTime(this string source, string format)
        {
            try
            {
                return DateTime.ParseExact(source.Trim(), format.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime ToDateTime(this string source, string[] format)
        {
            try
            {
                return DateTime.ParseExact(source.Trim(), format ?? Constants.PopularDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime ToDateTime(this object source) => source.ToTrim().ToDateTime();

        public static DateTime ToDateTime(this object source, string format) => source.ToTrim().ToDateTime(format);

        public static DateTime ToDateTime(this object source, string[] format) => source.ToTrim().ToDateTime(format);

        public static byte? ToByte(this string source, byte? defaultValue = null)
        {
            try
            {
                return byte.Parse(source.Trim().Replace(",", ""));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static short? ToShort(this string source, short? defaultValue = null)
        {
            try
            {
                return short.Parse(source.Trim().Replace(",", ""));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int? ToInt(this string source, int? defaultValue = null)
        {
            try
            {
                return int.Parse(source.Trim().Replace(",", ""));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long? ToLong(this string source, long? defaultValue = null)
        {
            try
            {
                return long.Parse(source.Trim().Replace(",", ""));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static decimal? ToDecimal(this string source, decimal? defaultValue = null)
        {
            try
            {
                var src = source.Split(".", SplitMode.Rigid); var numof = src.Length;
                switch (numof)
                {
                    case 0:
                        return decimal.Parse(source.ToTrim().Replace(",", ""));

                    case 2 when src[0].ToTrim() == "" && src[1].ToTrim() == "":
                        return 0;

                    case 2 when src[0].ToTrim() == "" && src[1].ToTrim() != "":
                        return decimal.Parse("0." + src[1].ToTrim());
                }
                var wholes = ""; var fractions = src[numof - 1].ToTrim();
                for (var i = 0; i < numof - 1; i++) wholes += src[i].ToTrim();
                return decimal.Parse((wholes + "." + fractions).Replace(",", ""));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static double? ToDouble(this string source, double? defaultValue = null)
        {
            try
            {
                var src = source.Split(".", SplitMode.Rigid); var numof = src.Length;
                switch (numof)
                {
                    case 0:
                        return double.Parse(source.ToTrim().Replace(",", ""));

                    case 2 when src[0].ToTrim() == "" && src[1].ToTrim() == "":
                        return 0;

                    case 2 when src[0].ToTrim() == "" && src[1].ToTrim() != "":
                        return double.Parse("0." + src[1].ToTrim());
                }
                var wholes = ""; var fractions = src[numof - 1].ToTrim();
                for (var i = 0; i < numof - 1; i++) wholes += src[i].ToTrim();
                return double.Parse((wholes + "." + fractions).Replace(",", ""));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static float? ToFloat(this string source, float? defaultValue = null)
        {
            try
            {
                var src = source.Split(".", SplitMode.Rigid); var numof = src.Length;
                switch (numof)
                {
                    case 0:
                        return float.Parse(source.ToTrim().Replace(",", ""));

                    case 2 when src[0].ToTrim() == "" && src[1].ToTrim() == "":
                        return 0;

                    case 2 when src[0].ToTrim() == "" && src[1].ToTrim() != "":
                        return float.Parse("0." + src[1].ToTrim());
                }
                var wholes = ""; var fractions = src[numof - 1].ToTrim();
                for (var i = 0; i < numof - 1; i++) wholes += src[i].ToTrim();
                return float.Parse((wholes + "." + fractions).Replace(",", ""));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static byte? ToByte(this object source, byte? defaultValue = null) => source.ToTrim().ToByte(defaultValue);

        public static short? ToShort(this object source, short? defaultValue = null) => source.ToTrim().ToShort(defaultValue);

        public static int? ToInt(this object source, int? defaultValue = null) => source.ToTrim().ToInt(defaultValue);

        public static long? ToLong(this object source, long? defaultValue = null) => source.ToTrim().ToLong(defaultValue);

        public static decimal? ToDecimal(this object source, decimal? defaultValue = null) => source.ToTrim().ToDecimal(defaultValue);

        public static double? ToDouble(this object source, double? defaultValue = null) => source.ToTrim().ToDouble(defaultValue);

        public static float? ToFloat(this object source, float? defaultValue = null) => source.ToTrim().ToFloat(defaultValue);

        public static string ToString(this byte? source, string format) => source.HasValue ? source.Value.ToString(format) : "";

        public static string ToString(this short? source, string format) => source.HasValue ? source.Value.ToString(format) : "";

        public static string ToString(this int? source, string format) => source.HasValue ? source.Value.ToString(format) : "";

        public static string ToString(this long? source, string format) => source.HasValue ? source.Value.ToString(format) : "";

        public static string ToString(this decimal? source, string format) => source.HasValue ? source.Value.ToString(format) : "";

        public static string ToString(this double? source, string format) => source.HasValue ? source.Value.ToString(format) : "";

        public static string ToString(this float? source, string format) => source.HasValue ? source.Value.ToString(format) : "";

        public static string ToString(this DateTime? source, string format, DisplayMode option = DisplayMode.None)
        {
            switch (option)
            {
                case DisplayMode.None:
                    return source.HasValue ? source.Value.ToString(format) : "";

                case DisplayMode.MinValue:
                    return source.HasValue ? source.Value.ToString(format) : DateTime.MinValue.ToString(format);

                case DisplayMode.MaxValue:
                    return source.HasValue ? source.Value.ToString(format) : DateTime.MaxValue.ToString(format);

                default:
                    return "";
            }
        }

        public static string Remove(this string source, char[] elements)
        {
            if (source == null) return "";
            if (elements == null || elements.Length == 0) return source;
            foreach (char c in elements) source = source.Replace(c.ToString(), "");
            return source;
        }

        public static string Remove(this string source, string[] elements)
        {
            if (source == null) return "";
            if (elements == null || elements.Length == 0) return source;
            foreach (string s in elements) source = source.Replace(s, "");
            return source;
        }

        public static string Remove(this string source, List<string> elements)
        {
            if (source == null) return "";
            if (elements == null || elements.Count == 0) return source;
            foreach (string s in elements) source = source.Replace(s, "");
            return source;
        }

        public static string Remove(this string source, string substr) => source != null ? substr != null ? source.Replace(substr, "") : source : "";

        public static string Substr(this string source, int start, int lenght) => source.NotNull() && start >= 0 && lenght >= 1 && start <= lenght - 1 ? source.Substring(start, lenght > source.Length ? source.Length : lenght) : "";

        public static string Left(this string source, int lenght) => source.Substr(0, lenght);

        public static string Right(this string source, int lenght) => source.NotNull() ? source.Length <= lenght ? source : source.Substr(source.Length - lenght, lenght) : "";

        public static string TakeWholesPart(this string source) => source.Split(".")[0].Trim();

        public static string TakeNumsPart(this string source)
        {
            var result = "";
            if (source.IsNull()) return result;
            var chr = new List<char>();
            var num = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            chr.AddRange(source.Where(c => num.Contains(c)).Select(c => c));
            chr.ForEach(c => result += c.ToString());
            return result;
        }

        public static string TakeCharsPart(this string source) => source.NotNull() ? source.Remove(source.TakeNumsPart().ToCharArray()) : "";

        public static DataTable ToDataTable<T>(this IEnumerable<T> colls)
        {
            var table = new DataTable();
            try
            {
                if (colls.IsNull()) return null;
                var props = TypeDescriptor.GetProperties(typeof(T));
                foreach (PropertyDescriptor prop in props)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in colls)
                {
                    var row = table.NewRow();
                    row.BeginEdit();
                    foreach (PropertyDescriptor prop in props)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    row.EndEdit();
                    table.Rows.Add(row);
                }
                return table;
            }
            catch
            {
                return table;
            }
        }

        public static List<T> ToList<T>(this DataTable source) where T : new()
        {
            var result = new List<T>();
            try
            {
                var columns = new List<string>();
                foreach (DataColumn dataColumn in source.Columns)
                    columns.Add(dataColumn.ColumnName);
                result = source.AsEnumerable().ToList().ConvertAll<T>(row => GetObject<T>(row, columns));
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static T GetObject<T>(DataRow row, List<string> columns) where T : new()
        {
            T obj = new T();
            try
            {
                var value = "";
                var colname = "";
                PropertyInfo[] properties;
                properties = typeof(T).GetProperties();
                foreach (PropertyInfo prop in properties)
                {
                    colname = columns.Find(name => name.ToLower() == prop.Name.ToLower());
                    if (!string.IsNullOrEmpty(colname))
                    {
                        value = row[colname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(prop.PropertyType) != null)
                            {
                                value = row[colname].ToString().Replace("$", "").Replace(",", "");
                                prop.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(prop.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[colname].ToString().Replace("%", "");
                                prop.SetValue(obj, Convert.ChangeType(value, Type.GetType(prop.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch
            {
                return obj;
            }
        }

        #endregion Convert Extensions

        #region DateTime Extensions

        /// <summary>
        /// Get the datetime of the start of the week.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek"></param>
        /// <returns></returns>
        /// <example>
        /// DateTime dt = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
        /// DateTime dt = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
        /// </example>
        /// <remarks></remarks>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0) diff += 7;
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Get the datetime of the start of the month.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime StartOfMonth(this DateTime dt) => new DateTime(dt.Year, dt.Month, 1);

        /// <summary>
        /// Get datetime of the start of the year.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime StartOfYear(this DateTime dt) => new DateTime(dt.Year, 1, 1);

        #endregion DateTime Extensions

        #region Other Extensions

        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (T element in array)
                action(element);
        }

        public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> source, Action<KeyValuePair<TKey, TValue>> action)
        {
            foreach (KeyValuePair<TKey, TValue> pair in source)
                action(pair);
        }

        public static void ForEach<TKey, TValue>(this SpecialDict<TKey, TValue> source, Action<KeyValuePair<TKey, TValue>> action)
        {
            foreach (KeyValuePair<TKey, TValue> pair in source.Dict)
                action(pair);
        }

        public static void ForEach<TKey, TValue>(this AbstractDict<TKey, TValue> source, Action<KeyValuePair<TKey, TValue>> action)
        {
            foreach (KeyValuePair<TKey, TValue> pair in source.Dict)
                action(pair);
        }

        public static void ImpKeysValues<TKey, TValue>(this Dictionary<TKey, TValue> source, List<TKey> keys, List<TValue> values)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keys == null) throw new ArgumentNullException("keys");
            if (values == null) throw new ArgumentNullException("values");
            List<TKey> distinct = keys.Distinct().ToList();
            int count = (distinct.Count <= values.Count) ? distinct.Count : values.Count;
            source.Clear(); for (int i = 0; i < count; i++) source.Add(distinct[i], values[i]);
        }

        public static byte[] ImageToBytes(this Image obj, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                obj.Save(ms, format);
                return ms.ToArray();
            }
        }

        #endregion Other Extensions
    }
}