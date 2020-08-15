using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JoinTheQueue.Core.Services;
using Newtonsoft.Json;

namespace JoinTheQueue.Infrastructure.Services
{
    public class WebHookService : IWebHookService
    {
        private readonly IHttpClientFactory _httpClient;

        public WebHookService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> TriggerWebHook(string url, object payload)
        {
            using var httpClient = _httpClient.CreateClient("WebHook");
            httpClient.BaseAddress = new Uri(url);
            var httpContent =
                new StringContent(JsonConvert.SerializeObject(payload, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new List<JsonConverter>
                    {
                        new Newtonsoft.Json.Converters.StringEnumConverter()
                    }
                }), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new HttpRequestException(
                $"Error {response.StatusCode} {response.Content} when querying {httpClient.BaseAddress}endpoint.");
        }
    }
}