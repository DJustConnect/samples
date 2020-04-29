using System.Collections.Generic;

namespace Ilvo.DataHub.Samples.Provider.Models
{
    public class SimpleFarmData
    {
        public string FarmId { get; set; }
        public object Data { get; set; }

        public static IList<SimpleFarmData> GetSomeData()
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
    }
}
