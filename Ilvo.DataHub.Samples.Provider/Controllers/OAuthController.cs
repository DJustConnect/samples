using Ilvo.DataHub.Samples.Provider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ilvo.DataHub.Samples.Provider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OAuthController : ControllerBase
    {
        [HttpGet("{farmId}")]
        public ActionResult<SimpleFarmData> GetOAuthData([FromRoute] string farmId)
        {
            return Ok(new SimpleFarmData
            {
                FarmId = farmId,
                Data = "testing OAuth"
            });
        }
    }
}