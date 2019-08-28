using CommandLine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Ilvo.DataHub.Samples.Consumer
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            return await Parser.Default.ParseArguments<ConsumerOptions>(args)
                    .MapResult(
                        (opts) => CallAPI(opts),
                        errs => Task.FromResult(-1));
        }

        private static async Task<int> CallAPI(ConsumerOptions opts)
        {
            var certificate = new X509Certificate();
            certificate.Import(opts.CertificatePath);

            var clientHandler = new HttpClientHandler();
            clientHandler.ClientCertificates.Add(certificate);
            clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;

            using (var client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", opts.SubscriptionKey);
                if (!string.IsNullOrEmpty(opts.Purpose))
                    client.DefaultRequestHeaders.Add("purpose", opts.Purpose);

                var request = new HttpRequestMessage(opts.Verb, opts.Url);
                var response = await client.SendAsync(request);

                Console.Write(JsonConvert.SerializeObject(response));
                return response.IsSuccessStatusCode ? 0 : 1;
            }
        }
    }
}
