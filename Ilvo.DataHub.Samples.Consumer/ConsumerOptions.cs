﻿using CommandLine;
using System;

namespace Ilvo.DataHub.Samples.Consumer
{
    [Verb("consume", HelpText = "consumes given api call")]
    public class ConsumerOptions
    {
        [Option('u', "url", Required = true, HelpText = "")]
        public Uri Url { get; set; }

        [Option('v', "verb", Required = true, HelpText = "HTTP verb")]
        public string Verb { get; set; }

        [Option('c', "certificate path", Required = true, HelpText = "path to the client certificate")]
        public string CertificatePath { get; set; }

        [Option('p', "certificate password", Required = true, HelpText = "password of the certificate")]
        public string CertificatePassword { get; set; }

        [Option('s', "subscriptionkey", Required = true, HelpText = "Subscriptionkey")]
        public string SubscriptionKey { get; set; }
    }
}
