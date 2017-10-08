using System;
using ProtoBuf;
using System.IO;

namespace Microsoft.Extensions.Caching.Distributed
{
    internal static class DistributedCacheExtensions
    {
        public static T Put<T>(this IDistributedCache cache, string cacheKey, Func<T> sourceGetter, DateTime absoluteExpiration) where T : class
        {
            return Get(
                (out T cacheData) =>
                {
                    cacheData = cache.Get<T>(cacheKey);
                    return cacheData != null;
                },
                sourceGetter,
                cacheData => cache.Set(cacheKey, cacheData, absoluteExpiration));
        }

        private delegate bool DataGetter<T>(out T data);

        private static T Get<T>(DataGetter<T> dataGetter, Func<T> sourceGetter, Action<T> dataSetter)
        {
            T data;
            if (dataGetter(out data))
            {
                return data;
            }
            data = sourceGetter();
            if (data != null)
            {
                dataSetter(data);
            }
            return data;
        }


        public static T Get<T>(this IDistributedCache cache, string key)
        {
            var data = cache.Get(key);
            if (data == null)
            {
                return default(T);
            }
            using (var stream = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }

        public static void Set<T>(this IDistributedCache cache, string key, T data, DateTime absoluteExpiration)
        {
            if (data == null) return;
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, data);
                cache.Set(key, stream.ToArray(), new DistributedCacheEntryOptions { AbsoluteExpiration = absoluteExpiration });
            }
        }
    }
}