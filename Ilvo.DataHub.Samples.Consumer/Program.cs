using CommandLine;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
                Console.Write(e);
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
                    client.DefaultRequestHeaders.Add("DjustConnect-Subscription-Key", opts.SubscriptionKey);

                    var request = new HttpRequestMessage(new HttpMethod(opts.Verb), opts.Url);
                    var response = await client.SendAsync(request);

                    var responseAsString = await response.Content.ReadAsStringAsync();
                    Console.Write(string.Join("\r\n", response.Headers.Select(h=> $"{h.Key}: {string.Join(", ", h.Value)}")));
                    Console.Write(JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseAsString), Formatting.Indented));
                    return response.IsSuccessStatusCode ? 0 : -1;
                }
            }
        }
    }
}
