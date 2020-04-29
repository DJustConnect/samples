using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ilvo.DataHub.Samples.Provider.Filters;
using Ilvo.DataHub.Samples.Provider.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ilvo.DataHub.Samples.Provider.Controllers
{
    [RequireClientCertificate]
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderFilteringController : ControllerBase
    {
        private readonly IHostingEnvironment _environment;
        public IHttpContextAccessor HttpContextAccessor { get; }

        public ProviderFilteringController(IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _environment = environment;
            HttpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SimpleFarmData>>> GetFarmData([FromQuery] Uri pdpUri)
        {
            var data = SimpleFarmData.GetSomeData();

            //Get correlation id from http context
            if (HttpContextAccessor.HttpContext.Request.Headers.TryGetValue("DJustConnect-Correlation-Id", out var correlationId))
            {
                //Create a http client with your provider certificate
                using (var certificate = new X509Certificate2($"{_environment.ContentRootPath}\\sample.provider.pfx", "YourCertificatePassword"))
                {
                    var clientHandler = new HttpClientHandler();
                    clientHandler.ClientCertificates.Add(certificate);
                    using (var httpClient = new HttpClient(clientHandler))
                    {
                        //Call the pdp api with all the farm ids that have data in the resource to get the list of approved farmIds
                        //These ids will be of the type your configured on your resource in DJust Connect
                        var response = await httpClient.PostAsync<PdpRequest>(pdpUri,
                            new PdpRequest
                            {
                                CorrelationId = new Guid(correlationId.ToString()),
                                FarmNumbers = data.Select(d => d.FarmId)
                            }, new JsonMediaTypeFormatter());

                        //Check if the pdp call was successful. If not return error
                        if (!response.IsSuccessStatusCode)
                            return StatusCode(StatusCodes.Status500InternalServerError);

                        //Get the list from farmIds from the response body
                        var approvedFarms = JsonConvert.DeserializeObject<IEnumerable<string>>(await response.Content.ReadAsStringAsync());

                        //Filter data based on the pdp response
                        return Ok(data.Where(d => approvedFarms.Contains(d.FarmId)));
                    }
                }
            }

            //When there is no correlation id return a bad request because there should be one
            return BadRequest();
        }
    }

    public class PdpRequest
    {
        public IEnumerable<string> FarmNumbers { get; set; }
        public Guid CorrelationId { get; set; }
    }
}