using CommandLine;
using Newtonsoft.Json;
using System;
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
                        TryCallAPI,
                        errs => Task.FromResult(-1));
        }

        private static async Task<int> TryCallAPI(ConsumerOptions opts)
        {
            try
            {
                return await CallAPI(opts);
            }
            catch (Exception e)
            {
                Console.Write(JsonConvert.SerializeObject(e));
                return -1;
            }
        }

        private static async Task<int> CallAPI(ConsumerOptions opts)
        {
            using (var certificate = new X509Certificate2(opts.CertificatePath, opts.CertificatePassword))
            {
                var clientHandler = new HttpClientHandler();
                clientHandler.ClientCertificates.Add(certificate);
                clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;

                using (var client = new HttpClient(clientHandler))
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", opts.SubscriptionKey);

                    var request = new HttpRequestMessage(new HttpMethod(opts.Verb), opts.Url);
                    var response = await client.SendAsync(request);

                    Console.Write(JsonConvert.SerializeObject(await response.Content.ReadAsStringAsync()));
                    return response.IsSuccessStatusCode ? 0 : -1;
                }
            }
        }
    }
}
