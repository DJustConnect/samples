using CommandLine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

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
            int pages = 0, currentPage = 0;
            var result = new List<DarStatusSummary>();
            do
            {
                var response = await client.GetAsync(new Uri($"https://partnerapi.djustconnect.cegeka.com/api/DarStatus?PageNumber={currentPage+1}&ResourceIdFilter={opts.ResourceId}&FarmNumberFilter={opts.Kbo}"));
                var responseAsString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Partner API returned {response.StatusCode} with message: {responseAsString}");
                    return result;
                }

                pages = Convert.ToInt32(response.Headers.GetValues("X-Pages").FirstOrDefault());
                currentPage = Convert.ToInt32(response.Headers.GetValues("X-PageNumber").FirstOrDefault());
                var pageSize = Convert.ToInt32(response.Headers.GetValues("X-PageSize").FirstOrDefault());
                var totalAmount = Convert.ToInt32(response.Headers.GetValues("X-TotalCount").FirstOrDefault());
                var data = JsonConvert.DeserializeObject<IEnumerable<DarStatusSummary>>(responseAsString);
                result.AddRange(data);
                Console.WriteLine($"Got data for page {currentPage}/{pages}");
            } while (currentPage > pages);

            return result;
        }

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
}
