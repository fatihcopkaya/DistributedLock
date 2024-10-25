using Microsoft.Extensions.Caching.Distributed;
using PaymentService.Abstracts;
using System.Text.Json;

namespace PaymentService.Services
{
    public class CacheService(IDistributedCache _cache) : ICacheService
    {
        public async Task<T> GetAsync<T>(string key)
        {
            var cacheItem = await _cache.GetStringAsync(key);

            if (cacheItem != null)
            {
                if (typeof(T).IsValueType)
                {
                    return (T)Convert.ChangeType(cacheItem, typeof(T));
                }
                else
                {
                    return JsonSerializer.Deserialize<T>(cacheItem);
                }
            }
            return default;
        }

        public async Task RefreshAsync(string key)
        {
            await _cache.RefreshAsync(key);
        }
        public async Task LockAndUnlock<T>(string key, T item)
        {
            var stringKey = await _cache.GetAsync(key);
            if (stringKey != null)
            {
                while (true)
                {

                }
            }
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T item, Action<CacheSettings> config)
        {
            string itemStringRepresentation;

            if (typeof(T).IsValueType)
            {
                itemStringRepresentation = item.ToString();
            }
            else
            {
                itemStringRepresentation = JsonSerializer.Serialize(item);
            }

            var cacheSettings = new CacheSettings();

            config(cacheSettings);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(cacheSettings.AbsoluteExpiration),
                SlidingExpiration = TimeSpan.FromMinutes(cacheSettings.SlidingExpiration)
            };

            await _cache.SetStringAsync(key, itemStringRepresentation, options);
        }
    }
}

