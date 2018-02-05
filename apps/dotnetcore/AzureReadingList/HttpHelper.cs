using AzureReadingCore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureReadingList
{
    public class HttpHelper
    {
        private string webUrl;
        private String responseString;

        public HttpHelper(string url)
        {
            webUrl = url;
        }

        public async Task<string> GetResponse()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Concat(Settings.EndPoint, webUrl));
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
            }

            return responseString;
        }

        public async Task<string> PostRequest(string _content)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, string.Concat(Settings.EndPoint, webUrl));
            request.Content = new StringContent(JsonConvert.SerializeObject(_content), Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await client.SendAsync(request);

            string responseString = string.Empty;

            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
            }

            return responseString;
        }
    }
}
