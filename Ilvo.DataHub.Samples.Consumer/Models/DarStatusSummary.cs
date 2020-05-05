using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ilvo.DataHub.Samples.Consumer.Models
{
    public class DarStatusSummary
    {
        public Guid PartnerId { get; set; }

        public string PartnerName { get; set; }

        public string FarmNumber { get; set; }

        public Guid ResourceId { get; set; }

        public string ResourceName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FarmStatus FarmStatus { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AccessRequestStatus ResourceStatus { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DarStatus DarStatus { get; set; }
    }

    public enum FarmStatus
    {
        HasUser = 0,
        NotFound = 1,
        HasNoUser = 2
    }

    public enum AccessRequestStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public enum DarStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        NotApplicable = 3,
        NoMapping = 4,
        NoData = 5
    }
}
