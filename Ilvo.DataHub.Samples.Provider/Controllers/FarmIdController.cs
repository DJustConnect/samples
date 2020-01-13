using System;
using System.Collections.Generic;
using System.Linq;
using Ilvo.DataHub.Samples.Provider.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Ilvo.DataHub.Samples.Provider.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] //Hides the API in the Swagger UI
    public class FarmIdController : ControllerBase
    {
        private readonly Dictionary<string, IEnumerable<string>> _resources;

        public FarmIdController(IHttpContextAccessor context)
        {
            var baseUrl = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}";
            _resources = new Dictionary<string, IEnumerable<string>>
            {
                {$"{baseUrl}/api/values", new []{ "100", "101", "102" }},
                {$"{baseUrl}/api/number", new []{ "100", "103", "104", "105" }},
                {$"{baseUrl}/api/number/pi", new []{ "105", "106" }},
                {$"{baseUrl}/api/word", new []{ "102", "104", "105", "106" }}
            };
        }

        [HttpPost]
        public IEnumerable<string> GetFarmIds([FromBody] FarmIdRequest request)
        {
            var defaultValue = default(KeyValuePair<string, IEnumerable<string>>);

            //Check if the resource exists. Otherwise return an empty array
            var resource = _resources.FirstOrDefault(u => request.ResourceUrl.Contains(u.Key, StringComparison.OrdinalIgnoreCase));
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