using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ilvo.DataHub.Samples.Provider.Filters;
using Microsoft.AspNetCore.Http;
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
        /// Returns all the values.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "word1", "word2", "word3", "word4" };
        }

        /// <summary>
        /// Returns certain the value.
        /// </summary>
        /// <param name="farmId"></param>
        /// <returns></returns>
        [HttpGet("{farmId}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public ActionResult<string> Get(string farmId)
        {
            return  "word1";
        }
    }
}