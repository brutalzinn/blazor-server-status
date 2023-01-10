using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace blazor_server_status
{
    public class RedisService
    {
        private readonly IDistributedCache _redisCache;

        public RedisService(IDistributedCache redisDatabase)
        {
            _redisCache = redisDatabase;
        }

        public T Get<T>(string chave)
        {
            var value = _redisCache.GetString(chave);

            if (value != null)
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;

        }

        public T Set<T>(string chave, T valor, TimeSpan expiracao)
        {
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
                SlidingExpiration = expiracao
            };

            _redisCache.SetString(chave, JsonSerializer.Serialize(valor), timeOut);
            return valor;
        }

        public T Set<T>(string chave, T valor)
        {
            _redisCache.SetString(chave, JsonSerializer.Serialize(valor));
            return valor;
        }

        public bool Clear(string chave)
        {
            _redisCache.Remove(chave);
            return default;
        }

        public bool Exists(string chave)
        {
            return _redisCache.Get(chave) != null;
        }

        /// <summary>
        /// Usar apenas para cache de listas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="chave"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public T ItemAdd<T>(string chave, T valor, TimeSpan expiracao)
        {
            var value = _redisCache.GetString(chave);
            if (value != null)
            {
                var lista = JsonSerializer.Deserialize<List<T>>(value);
                lista.Add(valor);
                Set(chave, JsonSerializer.Serialize(lista), expiracao);
            }
            else
            {
                var lista = new List<T>();
                lista.Add(valor);
                Set(chave, JsonSerializer.Serialize(lista), expiracao);
            }
            return default;
        }


        /// <summary>
        /// Usar apenas para cache de listas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="chave"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public void ItemRemove<T>(string chave, int index)
        {
            var value = _redisCache.GetString(chave);

            if (value != null)
            {
                var lista = JsonSerializer.Deserialize<List<T>>(value);
                lista.RemoveAt(index);
                Set(chave, JsonSerializer.Serialize(lista));
            }
        }
    }
}
