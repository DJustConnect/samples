using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Ilvo.DataHub.Samples.Provider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmDataController : ControllerBase
    {
        [HttpGet("simple")]
        public ActionResult<IEnumerable<SimpleFarmData>> GetSimpleFarmData()
        {
            return new List<SimpleFarmData>
            {
                new SimpleFarmData
                {
                    FarmId = "100",
                    Data = "Data for farm 100"
                },
                new SimpleFarmData
                {
                    FarmId = "200",
                    Data = "Data for farm 200"
                },
                new SimpleFarmData
                {
                    FarmId = "300",
                    Data = "Data for farm 300"
                },
                new SimpleFarmData
                {
                    FarmId = "400",
                    Data = "Data for farm 400"
                },                new SimpleFarmData
                {
                    FarmId = "500",
                    Data = "Data for farm 500"
                }
            };
        }

        [HttpGet("complex")]
        public ActionResult<IEnumerable<ComplexFarmData>> GetComplexFarmData()
        {
            return new List<ComplexFarmData>
            {
                new ComplexFarmData
                {
                    Regions = new List<Region>
                    {
                        new Region
                        {
                            Name = "West-Vlaanderen",
                            Cities = new List<City>
                            {
                                new City
                                {
                                    Name = "Kortrijk",
                                    FarmData = new List<FarmData>
                                    {
                                        new FarmData
                                        {
                                            Name = "Farm 1",
                                            ContactInfo = new ContactInfo
                                            {
                                                FarmId = "100",
                                                Address = "Kortrijk 1"
                                            }
                                        },
                                        new FarmData
                                        {
                                            Name = "Farm 2",
                                            ContactInfo = new ContactInfo
                                            {
                                                FarmId = "200",
                                                Address = "Kortrijk 2"
                                            }
                                        }
                                    }
                                },
                                new City
                                {
                                    Name = "Waregem",
                                    FarmData = new List<FarmData>
                                    {
                                        new FarmData
                                        {
                                            Name = "Farm 3",
                                            ContactInfo = new ContactInfo
                                            {
                                                FarmId = "300",
                                                Address = "Waregem 1"
                                            }
                                        },
                                        new FarmData
                                        {
                                            Name = "Farm 3",
                                            ContactInfo = new ContactInfo
                                            {
                                                FarmId = "400",
                                                Address = "Waregem 2"
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        new Region
                        {
                            Name = "Oost-Vlaanderen",
                            Cities = new List<City>
                            {
                                new City
                                {
                                    Name = "Gent",
                                    FarmData = new List<FarmData>
                                    {
                                        new FarmData
                                        {
                                            Name = "Farm 5",
                                            ContactInfo = new ContactInfo
                                            {
                                                FarmId = "500",
                                                Address = "Gent 1"
                                            }
                                        },
                                        new FarmData
                                        {
                                            Name = "Farm 6",
                                            ContactInfo = new ContactInfo
                                            {
                                                FarmId = "600",
                                                Address = "Gent 2"
                                            }
                                        }
                                    }
                                },
                                new City
                                {
                                    Name = "Lokeren",
                                    FarmData = new List<FarmData>
                                    {
                                        new FarmData
                                        {
                                            Name = "Farm 7",
                                            ContactInfo = new ContactInfo
                                            {
                                                FarmId = "700",
                                                Address = "Lokeren 1"
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        new Region
                        {
                            Name = "Antwerpen",
                            Cities = new List<City>
                            {
                                new City
                                {
                                    Name = "Antwerpen",
                                    FarmData = new List<FarmData>()
                                }
                            }
                        },
                        new Region
                        {
                            Name = "Limburg",
                            Cities = new List<City>
                            {
                                new City
                                {
                                    Name = "Hasselt",
                                    FarmData = new List<FarmData>
                                    {
                                        new FarmData
                                        {
                                            Name = "Farm 8",
                                            ContactInfo = null
                                        },
                                        new FarmData
                                        {
                                            Name = "Farm 9",
                                            ContactInfo = new ContactInfo
                                            {
                                                FarmId = null,
                                                Address = "Hasselt 2"
                                            }
                                        },
                                        new FarmData
                                        {
                                            Name = "Farm 10",
                                            ContactInfo = new ContactInfo
                                            {
                                                FarmId = "1000",
                                                Address = "Hasselt 3"
                                            }
                                        }
                                    }
                                }
                            }
                        },
                    }
                }
            };
        }
    }

    public class SimpleFarmData
    {
        public string FarmId { get; set; }
        public string Data { get; set; }
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