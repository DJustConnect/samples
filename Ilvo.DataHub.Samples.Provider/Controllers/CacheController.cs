using System.Collections.Generic;
using Ilvo.DataHub.Samples.Provider.Cache;
using Ilvo.DataHub.Samples.Provider.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ilvo.DataHub.Samples.Provider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly FarmIdCache _cache;

        public CacheController(FarmIdCache cache)
        {
            _cache = cache;
        }

        [HttpGet()]
        public Dictionary<string, IEnumerable<string>> GetFarmIdCache()
            => _cache.GetResources();

        [HttpPost()]
        public IActionResult AddResourceToCache([FromBody] FarmIdResource farmIdResource)
        {
            _cache.AddResource(farmIdResource.Resource, farmIdResource.FarmIds);
            return Ok();
        }

        [HttpPost("back-to-default")]
        public IActionResult ResetCache()
        {
            _cache.ResetCacheToDefault();
            return Ok();
        }
    }
}