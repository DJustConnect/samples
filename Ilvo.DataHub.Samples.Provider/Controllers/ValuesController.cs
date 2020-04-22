using Ilvo.DataHub.Samples.Provider.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Ilvo.DataHub.Samples.Provider.Controllers
{
    /*
     * Adding and configuring the OpenAPI spec and Swagger UI: https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.2&tabs=visual-studio
     * Configuring IISExpress to require client certificates: https://serverfault.com/a/900492. The choice to require or only allow client certificates to be received
     *  depends on your choice and what your host of choice offers.
     * Use the self-signed-client-cert.pfx; install into the CurrentUser/My and LocalMachine/CA stores. The "do not trust" certificate can be used to test accessing
     *  the application with a non-trusted client certificate; install it ONLY into the CurrentUser/My store.
     *  - CurrentUser/My: this will make the certificate available for selection in your browser
     *  - LocalMachine/CA: this explicitly trusts the certificate on your local machine, so IISExpress sees it as valid
     * More info on configuring Mutual SSL on Azure App Services: https://docs.microsoft.com/en-us/azure/app-service/app-service-web-configure-tls-mutual-auth
     */
    [RequireClientCertificate]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Forbidden)]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Returns values.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<string>> GetValues([FromQuery] string farmId)
        {
            if (string.IsNullOrEmpty(farmId))
                return new string[] { "value1", "value2" };
            return new string[] { "value1" };
        }

        /// <summary>
        /// Adds a new value.
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public void PostValue([FromBody] string value)
        {
        }

        /// <summary>
        /// Adds a value with a given ID, or modifies an existing value.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public void PutValue(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// Deletes an existing value.
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public void DeleteValue(int id)
        {
        }
    }
}
