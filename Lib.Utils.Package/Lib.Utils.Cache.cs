/// <summary>
/// For Desktop Application.
/// Do not perform debugging at here, because it will disrupt the memory cache process.
/// </summary>
namespace Lib.Utils.Package
{
    #region Using System Library (.NET Framework v4.5.2)

    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Threading.Tasks;

    #endregion Using System Library (.NET Framework v4.5.2)

    #region CACHE MANAGER

    public abstract class CachingProviderBase
    {
        public CachingProviderBase() => DeleteLog();

        protected MemoryCache cache = new MemoryCache("CachingProvider");
        private static readonly object padlock = new object();

        protected virtual void AddItem(string key, object value)
        {
            lock (padlock)
            {
                if (cache.NotNull())
                {
                    RemoveItem(key);
                    cache.Add(key, value, DateTimeOffset.MaxValue);
                }
                else
                {
                    WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": CachingProvider-AddItem: Cache is null.");
                }
            }
        }

        protected virtual void RemoveItem(string key)
        {
            lock (padlock)
            {
                if (cache.NotNull())
                {
                    if (cache.Contains(key)) cache.Remove(key);
                }
                else
                {
                    WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": CachingProvider-RemoveItem: Cache is null.");
                }
            }
        }

        protected virtual object GetItem(string key, bool remove)
        {
            lock (padlock)
            {
                var res = cache[key];
                if (res.NotNull())
                {
                    if (remove) cache.Remove(key);
                }
                else
                {
                    WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": CachingProvider-GetItem: Don't contains key: " + key + ".");
                }
                return res;
            }
        }

        protected virtual void Release()
        {
            lock (padlock)
            {
                if (cache.NotNull())
                {
                    cache.Dispose();
                }
                else
                {
                    WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": CachingProvider-Release: Cache is null.");
                }
            }
        }

        protected virtual void Clear()
        {
            lock (padlock)
            {
                if (cache.NotNull())
                {
                    var allKeys = cache.Select(o => o.Key);
                    Parallel.ForEach(allKeys, key => cache.Remove(key));
                }
                else
                {
                    WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": CachingProvider-Clear: Cache is null.");
                }
            }
        }

        #region Logging

        private const string LogFile = ".\\CachingProvider_Errors.txt";

        protected void DeleteLog()
        {
            if (File.Exists(LogFile)) File.Delete(LogFile);
        }

        protected void WriteLog(string content)
        {
            using (TextWriter tw = File.AppendText(LogFile))
            {
                tw.WriteLine(content);
                tw.Flush();
                tw.Close();
            }
        }

        #endregion Logging
    }

    public interface IGlobalCachingProvider
    {
        void AddItem(string key, object value);

        void RemoveItem(string key);

        object GetItem(string key);

        void Release();

        void Clear();
    }

    public class GlobalCachingProvider : CachingProviderBase, IGlobalCachingProvider
    {
        #region Singleton

        protected GlobalCachingProvider()
        {
        }

        public static GlobalCachingProvider Instance => Nested.instance;

        private class Nested
        {
            /// <summary>
            /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit.
            /// </summary>
            static Nested() { }

            internal static readonly GlobalCachingProvider instance = new GlobalCachingProvider();
        }

        #endregion Singleton

        #region ICachingProvider

        public virtual new void AddItem(string key, object value) => base.AddItem(key, value);

        public virtual object GetItem(string key) => base.GetItem(key, true);

        public virtual new object GetItem(string key, bool remove) => base.GetItem(key, remove);

        public virtual new void RemoveItem(string key) => base.RemoveItem(key);

        public virtual new void Release() => base.Release();

        public virtual new void Clear() => base.Clear();

        #endregion ICachingProvider
    }

    #endregion CACHE MANAGER

    #region CACHE GLOBAL

    public sealed class GlobalCache
    {
        public static void Release()
        {
            try { GlobalCachingProvider.Instance.Clear(); }
            catch { }
        }

        public static void RemoveItem(string key)
        {
            try { GlobalCachingProvider.Instance.RemoveItem(key); }
            catch { }
        }

        public static void AddItem(string key, object value)
        {
            try { GlobalCachingProvider.Instance.AddItem(key, value); }
            catch { }
        }

        public static object GetItem(string key, bool remove = false)
        {
            try { return GlobalCachingProvider.Instance.GetItem(key, remove); }
            catch { return new object(); }
        }

        public static DataTable GetCacheTable(string table) => GetItem(table) as DataTable;
    }

    #endregion CACHE GLOBAL
}