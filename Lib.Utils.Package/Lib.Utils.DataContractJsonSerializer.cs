namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framework v4.5.2)

    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;

    #endregion Using System Library (.NET Framework v4.5.2)

    public interface ISerializer
    {
        string Serialize<T>(T obj) where T : class, new();

        T Deserialize<T>(string json) where T : class, new();
    }

    public sealed class JsonSerializer : ISerializer
    {
        public JsonSerializer() { }

        public string Serialize<T>(T obj) where T : class, new()
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    serializer.WriteObject(ms, obj);
                    return ms.ToArray().UTF8GetString();
                }
            }
            catch { }
            return "";
        }

        public T Deserialize<T>(string json) where T : class, new()
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                using (var stream = new MemoryStream(json.UTF8GetBytes()))
                {
                    return serializer.ReadObject(stream) as T;
                }
            }
            catch { }
            return new T();
        }
    }

    #region TESTING FOR SERIALIZE WITH NAMED OF ENUM

    internal enum TestEnum
    {
        FirstValue = 1,
        SecondValue = 2
    }

    [DataContract]
    internal class TestClass
    {
        [DataMember(Name = "Description", Order = 0)]
        public string Description { get; set; }

        public TestEnum EnumVal { get; set; }

        [DataMember(Name = "EnumVal", Order = 1)]
        private string EnumValString
        {
            get { return Enum.GetName(typeof(TestEnum), EnumVal); }
            set { EnumVal = (TestEnum)Enum.Parse(typeof(TestEnum), value, true); }
        }
    }

    #endregion
}