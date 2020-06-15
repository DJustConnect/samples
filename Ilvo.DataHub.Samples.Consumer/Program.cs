using CommandLine;
using Ilvo.DataHub.Samples.Consumer.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Ilvo.DataHub.Samples.Consumer
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            return await Parser.Default.ParseArguments<ConsumerOptions, StatusOptions>(args)
                    .MapResult(
                        (ConsumerOptions opts) => TryCallAPI(opts),
                        (StatusOptions opts) => TryGetStatus(opts),
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

        private static async Task<int> TryGetStatus(StatusOptions opts)
        {
            try
            {
                return await GetStatus(opts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        private static async Task<int> GetStatus(StatusOptions opts)
        {
            using (var certificate = new X509Certificate2(opts.CertificatePath, opts.CertificatePassword))
            {
                var clientHandler = new HttpClientHandler();
                clientHandler.ClientCertificates.Add(certificate);
                clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;

                using (var client = new HttpClient(clientHandler))
                {
                    Console.Write(JsonConvert.SerializeObject(await GetDarsSummary(client, opts), Formatting.Indented));
                    return 0;
                }
            }
        }

        private static async Task<IEnumerable<DarStatusSummary>> GetDarsSummary(HttpClient client, StatusOptions opts)
        {
            int pages = 0, currentPage = 0, totalAmount = 0;
            var result = new List<DarStatusSummary>();
            do
            {
                var parameters = GetParameters(opts, currentPage);
                var url = QueryHelpers.AddQueryString("https://partnerapi.djustconnect.be/api/DarStatus", parameters);
                var response = await client.GetAsync(url);
                var responseAsString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Partner API returned {response.StatusCode} with message: {responseAsString}");
                    return result;
                }

                pages = Convert.ToInt32(response.Headers.GetValues("X-Pages").FirstOrDefault());
                currentPage = Convert.ToInt32(response.Headers.GetValues("X-PageNumber").FirstOrDefault());
                totalAmount = Convert.ToInt32(response.Headers.GetValues("X-TotalCount").FirstOrDefault());
                var pageSize = Convert.ToInt32(response.Headers.GetValues("X-PageSize").FirstOrDefault());
                var data = JsonConvert.DeserializeObject<IEnumerable<DarStatusSummary>>(responseAsString);

                result.AddRange(data);
                Console.WriteLine($"Got data for page {currentPage}/{pages}");
            } while (currentPage < pages && totalAmount > 0);

            return result;
        }

        private static Dictionary<string, string> GetParameters(StatusOptions opts, int currentPage)
        {
            var parameters = new Dictionary<string, string>();

            if (opts.DarStatus.HasValue)
                parameters.Add("DarStatusFilter", opts.DarStatus.ToString());
            if (opts.ResourceStatus.HasValue)
                parameters.Add("ResourceStatusFilter", opts.ResourceStatus.ToString());
            if (opts.ResourceId.HasValue)
                parameters.Add("ResourceIdFilter", opts.ResourceId.ToString());
            if (!string.IsNullOrEmpty(opts.Kbo))
                parameters.Add("FarmNumberFilter", opts.Kbo);

            parameters.Add("PageNumber", (currentPage +1).ToString());
            return parameters;
        }
    }
}
