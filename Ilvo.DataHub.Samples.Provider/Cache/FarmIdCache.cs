using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Ilvo.DataHub.Samples.Provider.Cache
{
    public class FarmIdCache
    {
        private Dictionary<string, IEnumerable<string>> _cache;
        private readonly string _baseUrl;

        public FarmIdCache(IHttpContextAccessor context)
        {
            _baseUrl = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}";
            ResetCacheToDefault();
        }

        public Dictionary<string, IEnumerable<string>> GetResources()
            => _cache;

        public void AddResource(string resource, IEnumerable<string> farmIds)
        {
            if (_cache.ContainsKey(resource))
                _cache[resource] = farmIds;
            else
                _cache.Add(resource, farmIds);
        }

        public void ResetCacheToDefault()
        {
            _cache = new Dictionary<string, IEnumerable<string>>
            {
                {$"{_baseUrl}/api/values", new []{ "100", "101", "102" }},
                {$"{_baseUrl}/api/number", new []{ "100", "103", "104", "105" }},
                {$"{_baseUrl}/api/number/pi", new []{ "105", "106" }},
                {$"{_baseUrl}/api/word", new []{ "100", "102", "104", "105", "106" }},
                {$"{_baseUrl}/api/farmData/simple", new []{ "100", "200", "300", "400", "500" }},
                {$"{_baseUrl}/api/farmData/complex", new []{ "100", "200", "300", "400", "500", "600", "700", "1000" }}
            };
        }
    }
}
