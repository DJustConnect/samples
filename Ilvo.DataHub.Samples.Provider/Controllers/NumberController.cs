﻿using System;
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
    public class NumberController : ControllerBase
    {
        /// <summary>
        /// Returns all the values.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<int>), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<int>> GetNumbers()
        {
            return new int[] { 1, 12, 35 };
        }

        /// <summary>
        /// Returns pi.
        /// </summary>
        /// <returns></returns>
        [HttpGet("pi")]
        [ProducesResponseType(typeof(IEnumerable<int>), (int)HttpStatusCode.OK)]
        public ActionResult<double> GetPi()
        {
            return Math.PI;
        }
    }
}