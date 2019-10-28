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
                {$"{baseUrl}/values", new []{ "100", "101", "102" }},
                {$"{baseUrl}/number", new []{ "100", "103", "104", "105" }},
                {$"{baseUrl}/number/pi", new []{ "105", "106" }},
                {$"{baseUrl}/word", new []{ "102", "104", "105", "106" }}
            };
        }

        [HttpPost]
        public IEnumerable<string> GetFarmIds([FromBody] FarmIdRequest request)
        {
            //Check if the resource exists. Otherwise return an empty array
            var doesResourceExist = _resources.ContainsKey(request.ResourceUrl);
            if (!doesResourceExist)
                return new string[0];

            //Get the farm IDs for the resource
            var resource = _resources[request.ResourceUrl];

            //If all the resources are requested return them all
            if (request.All)
                return resource;

            //Else check what ids are present in both the arrays and return those. If there are none, return an empty array
            return resource.Intersect(request.FarmIds);
        }
    }
}