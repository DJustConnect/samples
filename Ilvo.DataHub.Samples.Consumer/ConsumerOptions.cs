using CommandLine;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Ilvo.DataHub.Samples.Consumer
{
    [Verb("consume", HelpText = "consumes given api call")]
    public class ConsumerOptions
    {
        [Option('u', "url", Required = true, HelpText = "")]
        public Uri Url { get; set; }

        [Option('v', "verb", Required = true, HelpText = "HTTP verb")]
        public HttpMethod Verb { get; set; }

        [Option('c', "certificate path", Required = true, HelpText = "path to the client certificate")]
        public string CertificatePath { get; set; }

        [Option('s', "subscriptionkey", Required = true, HelpText = "Subscriptionkey")]
        public string SubscriptionKey { get; set; }

        [Option('p', "purpose", Required = true, HelpText = "purpose")]
        public string Purpose { get; set; }
    }
}
