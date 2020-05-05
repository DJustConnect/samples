using System;
using CommandLine;
using Ilvo.DataHub.Samples.Consumer.Models;

namespace Ilvo.DataHub.Samples.Consumer
{
    [Verb("status", HelpText = "gets the status of data access requests by resourceId and farm kbo number")]
    public class StatusOptions
    {
        [Option('r', "resourceId", Required = false, HelpText = "identifier of the api resource")]
        public Guid? ResourceId { get; set; }

        [Option('k', "farm kbo number", Required = false, HelpText = "kbo number of the farm")]
        public string Kbo { get; set; }

        [Option('a', "resource access request status", Required = false, HelpText = "status of the resource access request")]
        public AccessRequestStatus? ResourceStatus { get; set; }

        [Option('d', "data access request status", Required = false, HelpText = "status of the data access request")]
        public DarStatus? DarStatus { get; set; }

        [Option('c', "consumer certificate path", Required = true, HelpText = "path to the consumer client certificate")]
        public string CertificatePath { get; set; }

        [Option('f', "consumer certificate password", Required = true, HelpText = "password of the consumer certificate")]
        public string CertificatePassword { get; set; }
    }
}
