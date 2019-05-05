using JHilburnFabricManager.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JHilburnFabricManager.Services
{
    public static class ApiClient
    {
        public static async Task<T> GetById<T>(string url, Dictionary<string, string> tokens)
        {
            using (var client = new HttpClient())
            {
                AddTokens(client, tokens);

                var apiResponse = await client.GetAsync(url);
                apiResponse.EnsureSuccessStatusCode();
                apiResponse.Headers.TryGetValues("X-Total-Count", out var countStr);
                
                var resultContentString = await apiResponse.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(resultContentString);
                return result;
            }
        }

        public static async Task<ApiPagedResponse<T>> Get<T>(string url, Dictionary<string, string> tokens, int page, int perPage)
        {
            using (var client = new HttpClient())
            {
                AddTokens(client, tokens);

                var apiResponse = await client.GetAsync(url);
                apiResponse.EnsureSuccessStatusCode();
                apiResponse.Headers.TryGetValues("X-Total-Count", out var countStr);

                var result = new ApiPagedResponse<T>();
                if (countStr.Any() && int.TryParse(countStr.First(), out var count))
                {
                    result.Count = count;
                }
                result.Page = page;
                result.PerPage = perPage;

                var resultContentString = await apiResponse.Content.ReadAsStringAsync();                
                result.Content = JsonConvert.DeserializeObject<IEnumerable<T>>(resultContentString);
                return result;
            }
        }

        public static async Task<T> Post<T>(string url, T contentValue, Dictionary<string, string> tokens)
        {
            using (var client = new HttpClient())
            {
                AddTokens(client, tokens);

                var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");
                var result = await client.PostAsync(url, content);
                result.EnsureSuccessStatusCode();
                string resultContentString = await result.Content.ReadAsStringAsync();
                T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);
                return resultContent;
            }
        }

        public static async Task<T> Put<T>(string url, T contentValue, Dictionary<string, string> tokens)
        {
            using (var client = new HttpClient())
            {
                AddTokens(client, tokens);

                var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");
                var result = await client.PutAsync(url, content);
                result.EnsureSuccessStatusCode();
                string resultContentString = await result.Content.ReadAsStringAsync();
                T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);
                return resultContent;
            }
        }

        public static async Task<T> Patch<T>(string url, T contentValue, Dictionary<string, string> tokens)
        {
            using (var client = new HttpClient())
            {
                AddTokens(client, tokens);

                var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");
                var result = await client.PatchAsync(url, content);
                result.EnsureSuccessStatusCode();
                string resultContentString = await result.Content.ReadAsStringAsync();
                T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);
                return resultContent;
            }
        }

        public static async Task Delete<T>(string url, Dictionary<string, string> tokens)
        {
            using (var client = new HttpClient())
            {
                AddTokens(client, tokens);

                var result = await client.DeleteAsync(url);
                result.EnsureSuccessStatusCode();
            }
        }

        private static void AddTokens(HttpClient client, Dictionary<string, string> tokens)
        {
            foreach (var t in tokens)
            {
                client.DefaultRequestHeaders.Add(t.Key, t.Value);
            }
        }
    }
}
