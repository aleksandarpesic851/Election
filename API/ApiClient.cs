using Election.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Election.API
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private Uri BaseEndpoint { get; set; }
        // Requested API service require new cookie, it will be added using this variable. now it's needed for authentication
        protected string AuthCookie { get; set; }
        public ApiClient()
        {
            BaseEndpoint = new Uri(Global.API_BASIC_URL);
            if (BaseEndpoint == null)
            {
                throw new ArgumentNullException("baseEndpoint");
            }
            _httpClient = new HttpClient();
        }

        /// <summary>  
        /// Common method for making GET calls  
        /// </summary>  
        protected async Task<Message<T>> GetAsync<T>(Uri requestUrl)
        {
            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            //response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            Message<T> results = JsonConvert.DeserializeObject<Message<T>>(data);
            results.StatusCode = response.StatusCode;
            return results;
        }

        /// <summary>  
        /// Common method for making POST calls  
        /// </summary>  
        protected async Task<Message<T>> PostAsync<T>(Uri requestUrl, T content)
        {
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            //response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            Message<T> results = JsonConvert.DeserializeObject<Message<T>>(data);
            results.StatusCode = response.StatusCode;
            return results;
        }
        protected async Task<Message<T1>> PostAsync<T1, T2>(Uri requestUrl, T2 content)
        {
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(content));
            //response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            Message<T1> results = JsonConvert.DeserializeObject<Message<T1>>(data);
            results.StatusCode = response.StatusCode;
            return results;
        }

        protected Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            relativePath = string.Format(System.Globalization.CultureInfo.InvariantCulture, relativePath);
            var endpoint = new Uri(BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }

        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

    }
}
