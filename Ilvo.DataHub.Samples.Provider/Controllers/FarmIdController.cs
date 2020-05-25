using System;
using System.Collections.Generic;
using System.Linq;
using Ilvo.DataHub.Samples.Provider.Cache;
using Ilvo.DataHub.Samples.Provider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ilvo.DataHub.Samples.Provider.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] //Hides the API in the Swagger UI
    public class FarmIdController : ControllerBase
    {
        private readonly FarmIdCache _cache;

        public FarmIdController(FarmIdCache cache)
        {
            _cache = cache;
        }

        [HttpPost]
        [Authorize]
        public IEnumerable<string> GetFarmIds([FromBody] FarmIdRequest request)
        {
            var defaultValue = default(KeyValuePair<string, IEnumerable<string>>);

            //Check if the resource exists. Otherwise return an empty array
            var resource = _cache.GetResources().FirstOrDefault(u => request.ResourceUrl.Contains(u.Key, StringComparison.OrdinalIgnoreCase));
            if (resource.Equals(defaultValue))
                return new string[0];

            //If all the resources are requested return them all
            if (request.All)
                return resource.Value;

            //Else check what ids are present in both the arrays and return those. If there are none, return an empty array
            return resource.Value.Intersect(request.FarmIds);
        }
    }
}