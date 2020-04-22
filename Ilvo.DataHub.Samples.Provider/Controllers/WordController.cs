using System.Collections.Generic;
using System.Net;
using Ilvo.DataHub.Samples.Provider.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Ilvo.DataHub.Samples.Provider.Controllers
{
    [RequireClientCertificate]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Forbidden)]
    public class WordController : ControllerBase
    {
        /// <summary>
        /// Returns a word.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{farmId}")]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<string>> GetWords([FromRoute] string farmId)
        {
            if (string.IsNullOrEmpty(farmId))
                return new string[] { "word1", "word2", "word3", "word4" };
            return new string[] { "word1" };
        }
    }
}