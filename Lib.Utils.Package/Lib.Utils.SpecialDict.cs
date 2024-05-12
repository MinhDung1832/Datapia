namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framework v4.5.2)

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;

    #endregion Using System Library (.NET Framework v4.5.2)

    [Serializable()]
    public sealed class SpecialDict<TKey, TValue> : AbstractDict<TKey, TValue>, ICloneable
    {
        public SpecialDict() : base() => Clear();

        public SpecialDict(Dictionary<TKey, TValue> dict)
        {
            Clear();
            dict.ForEach(x => Add(x.Key, x.Value));
        }

        public SpecialDict(SpecialDict<TKey, TValue> dict)
        {
            Clear();
            dict.ForEach(x => Add(x.Key, x.Value));
        }

        public SpecialDict(List<TKey> keys, List<TValue> values) => ImpKeysValues(keys, values);

        public override bool Equals(object obj)
        {
            var dict = obj as SpecialDict<TKey, TValue>;
            if (dict.IsNull()) return false;
            return Converter.Compare(Dict, dict.Dict);
        }

        public override int GetHashCode() => base.GetHashCode();

        public object Clone() => new SpecialDict<TKey, TValue>(this);

        public string Serialize() => new JsonSerializer().Serialize(Dict);

        public void Deserialize(string json) => Dict = new JsonSerializer().Deserialize<Dictionary<TKey, TValue>>(json);

        public void ImpKeysValues(List<TKey> keys, List<TValue> values)
        {
            var dict = new Dictionary<TKey, TValue>();
            dict.ImpKeysValues(keys, values);
            Clear();
            Dict = new Dictionary<TKey, TValue>(dict);
        }

        public static bool operator ==(SpecialDict<TKey, TValue> A, SpecialDict<TKey, TValue> B) => (A.IsNull() || B.IsNull()) ? false : A.Equals(B);

        public static bool operator !=(SpecialDict<TKey, TValue> A, SpecialDict<TKey, TValue> B) => !(A == B);
    }

    [DataContract()]
    public abstract class AbstractDict<TKey, TValue> : ISpecialDict<TKey, TValue>
    {
        [DataMember(Name = "Json")]
        public Dictionary<TKey, TValue> Dict = new Dictionary<TKey, TValue>();

        [DataMember(Name = "Count")]
        public int Count => Dict.Count;

        public bool HasVal => Count > 0;

        public List<TKey> Keys => Dict.Keys.ToList();

        public List<TValue> Values => Dict.Values.ToList();

        public TValue this[TKey key]
        {
            get { return Get(key); }
            set { Add(key, value); }
        }

        public virtual void Add(TKey key, TValue value)
        {
            Remove(key);
            Dict.Add(key, value);
        }

        public virtual bool Remove(TKey key) => Dict.Remove(key);

        public virtual TValue Get(TKey key) => ContainsKey(key) ? Dict[key] : default;

        public virtual bool ContainsKey(TKey key) => Dict.ContainsKey(key);

        public virtual bool ContainsValue(TValue value) => Dict.ContainsValue(value);

        public virtual List<TKey> KeysAsc()
        {
            List<TKey> keys = Keys;
            keys = keys.OrderBy(x => x).ToList();
            return keys;
        }

        public virtual List<TKey> KeysDesc()
        {
            List<TKey> keys = Keys;
            keys = keys.OrderByDescending(x => x).ToList();
            return keys;
        }

        public virtual List<TValue> ValuesOfKeysAsc()
        {
            List<TKey> keys = KeysAsc();
            List<TValue> values = new List<TValue>();
            keys.ForEach(x => values.Add(Get(x)));
            return values;
        }

        public virtual List<TValue> ValuesOfKeysDesc()
        {
            List<TKey> keys = KeysDesc();
            List<TValue> values = new List<TValue>();
            keys.ForEach(x => values.Add(Get(x)));
            return values;
        }

        public virtual void SortKeysAsc()
        {
            List<TKey> keys = KeysAsc();
            List<TValue> values = ValuesOfKeysAsc();
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            dict.ImpKeysValues(keys, values);
            Clear();
            Dict = new Dictionary<TKey, TValue>(dict);
        }

        public virtual void SortKeysDesc()
        {
            List<TKey> keys = KeysDesc();
            List<TValue> values = ValuesOfKeysDesc();
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            dict.ImpKeysValues(keys, values);
            Clear();
            Dict = new Dictionary<TKey, TValue>(dict);
        }

        public virtual void Clear() => Dict.Clear();
    }

    public interface ISpecialDict<TKey, TValue>
    {
        void Add(TKey key, TValue value);

        bool Remove(TKey key);

        TValue Get(TKey key);

        bool ContainsKey(TKey key);

        bool ContainsValue(TValue value);

        List<TKey> KeysAsc();

        List<TKey> KeysDesc();

        List<TValue> ValuesOfKeysAsc();

        List<TValue> ValuesOfKeysDesc();

        void SortKeysAsc();

        void SortKeysDesc();

        void Clear();
    }
}