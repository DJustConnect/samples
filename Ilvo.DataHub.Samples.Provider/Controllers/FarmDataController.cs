using System.Collections.Generic;
using Ilvo.DataHub.Samples.Provider.Filters;
using Ilvo.DataHub.Samples.Provider.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ilvo.DataHub.Samples.Provider.Controllers
{
    [RequireClientCertificate]
    [Route("api/[controller]")]
    [ApiController]
    public class FarmDataController : ControllerBase
    {
        [HttpGet("simple")]
        public ActionResult<IEnumerable<SimpleFarmData>> GetSimpleFarmData()
        {
            return Ok(SimpleFarmData.GetSomeData());
        }

        [HttpGet("complex")]
        public ActionResult<IEnumerable<ComplexFarmData>> GetComplexFarmData()
        {
            return JsonConvert.DeserializeObject<List<ComplexFarmData>>(System.IO.File.ReadAllText(@"Data\complex.json"));
        }
    }

    public class ComplexFarmData
    {
        public IEnumerable<Region> Regions { get; set; }
    }

    public class Region
    {
        public string Name { get; set; }
        public IEnumerable<City> Cities { get; set; }
    }

    public class City
    {
        public string Name { get; set; }
        public IEnumerable<FarmData> FarmData { get; set; }
    }

    public class FarmData
    {
        public string Name { get; set; }
        public ContactInfo ContactInfo { get; set; }
    }

    public class ContactInfo
    {
        public string Address { get; set; }
        public string FarmId { get; set; }
    }
}