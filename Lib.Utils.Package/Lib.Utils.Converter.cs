namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framework v4.5.2)

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    #endregion Using System Library (.NET Framework v4.5.2)

    public sealed class Converter
    {
        public Converter() { }

        public static bool Compare<TKey, TValue>(Dictionary<TKey, TValue> A, Dictionary<TKey, TValue> B)
        {
            if (A == null) return false;
            if (B == null) return false;
            if (A.Count != B.Count) return false;
            if (A.Keys.SequenceEqual(B.Keys) == false) return false;
            List<int> Compares = new List<int>();
            List<TKey> Keys = A.Keys.ToList();
            Keys.ForEach(x => Compares.Add(Comparer.Default.Compare(A[x], B[x])));
            Compares = Compares.Where(x => x == 0).ToList();
            return Compares.Count == A.Count;
        }

        public static string ToJson<T>(T obj) where T : class, new()
        {
            var js = new JsonSerializer();
            return js.Serialize(obj);
        }

        public static T ParseJson<T>(string json) where T : class, new()
        {
            var js = new JsonSerializer();
            return js.Deserialize<T>(json);
        }

        public static byte[] Zip<T>(T obj) where T : class, new()
        {
            try
            {
                var js = new JsonSerializer();
                var content = js.Serialize(obj);
                var compressed = CompressString(content);
                return Encoding.UTF8.GetBytes(compressed);
            }
            catch
            {
                return null;
            }
        }

        public static T Unzip<T>(byte[] obj) where T : class, new()
        {
            try
            {
                var js = new JsonSerializer();
                var compressed = Encoding.UTF8.GetString(obj);
                var content = DecompressString(compressed);
                return js.Deserialize<T>(content);
            }
            catch
            {
                return null;
            }
        }

        public static string CompressString(string content)
        {
            try
            {
                if (content.IsNull()) return "";
                byte[] buffer = content.UTF8GetBytes();
                var memoryStream = new MemoryStream();
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gZipStream.Write(buffer, 0, buffer.Length);
                }
                memoryStream.Position = 0;
                var compressedData = new byte[memoryStream.Length];
                memoryStream.Read(compressedData, 0, compressedData.Length);
                var gZipBuffer = new byte[compressedData.Length + 4];
                Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
                return gZipBuffer.ToBase64String();
            }
            catch
            {
                return "";
            }
        }

        public static string DecompressString(string content)
        {
            try
            {
                if (content.IsNull()) return "";
                byte[] gZipBuffer = content.FromBase64String();
                using (var memoryStream = new MemoryStream())
                {
                    int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                    memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);
                    var buffer = new byte[dataLength];
                    memoryStream.Position = 0;
                    using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        gZipStream.Read(buffer, 0, buffer.Length);
                    }
                    return buffer.UTF8GetString();
                }
            }
            catch
            {
                return "";
            }
        }

        public static string SerializeObject<T>(T obj)
        {
            if (obj == null) return "";
            using (var writer = new Utf8StringWriter())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public static T DeserializeObject<T>(string xmlstr)
        {
            if (xmlstr == null) return default(T);
            using (var reader = new StringReader(xmlstr))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }

        public static string DeserializeString(string xmlstr)
        {
            if (xmlstr == null) return "";
            using (var reader = new StringReader(xmlstr))
            {
                var serializer = new XmlSerializer(typeof(string));
                return (string)serializer.Deserialize(reader);
            }
        }
    }

    internal class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}