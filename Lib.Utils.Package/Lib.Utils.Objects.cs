namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framework v4.5.2)

    using System.Collections.Generic;

    #endregion Using System Library (.NET Framework v4.5.2)

    public class ComBox
    {
        public string Value { get; set; }

        public string Text { get; set; }
    }
    public class ComBox2
    {
        public string id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }
    }

    public static class ComBoxEx
    {
        public static string GetText(this List<ComBox> source, string value)
        {
            string empty = string.Empty;
            if (source.IsNull()) return empty;
            foreach (var item in source) if (item.Value == value) return item.Text;
            return empty;
        }

        public static string GetValue(this List<ComBox> source, string text, bool relative = true)
        {
            string empty = string.Empty;
            if (source.IsNull()) return empty;
            if (relative) { foreach (var item in source) if (item.Text != null && text != null && item.Text.ToLower() == text.ToLower()) return item.Value; }
            else { foreach (var item in source) if (item.Text == text) return item.Value; }
            return empty;
        }
    }
}