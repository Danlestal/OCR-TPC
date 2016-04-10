using System;
using System.Drawing.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace OCR
{
    public class APIClient
    {
        public string HostAddress { get; }

        public APIClient(string hostAddress)
        {
            HostAddress = hostAddress;
        }

        public APIResponse Get(string url)
        {
            var client = GetHttpClient();
            var response = client.GetAsync(HostAddress + url);
            return new APIResponse(response.Result);

        }

        public APIResponse Get(string url, int value)
        {
            var client = GetHttpClient();
            var response = client.GetAsync(HostAddress + url + "/" + value);
            return new APIResponse(response.Result);
        }

        public APIResponse Get(string url, string parameter)
        {
            var client = GetHttpClient();
            var response = client.GetAsync(HostAddress + url + "/" + parameter);
            return new APIResponse(response.Result);
        }

        public async Task<APIResponse> GetAsync(string url, string parameter)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync(HostAddress + url + "/" + parameter);
            return new APIResponse(response);
        }

        public APIResponse Post(string url, object data)
        {
            var client = GetHttpClient();
            var response = client.PostAsync(HostAddress + url, CreateContent(data));
            return new APIResponse(response.Result);
        }

        public APIResponse PostStream(string url, Stream data)
        {
            var client = GetHttpClient();
            var response = client.PostAsync(HostAddress + url, CreateStreamContent(data));
            return new APIResponse(response.Result);
        }

        public async Task<APIResponse> PostAsync(string url, object data)
        {
            var client = GetHttpClient();
            var response = await client.PostAsync(url, CreateContent(data));
            return new APIResponse(response);
        }

        private HttpClient GetHttpClient()
        {
            return new HttpClient();
        }
        
        private StringContent CreateContent(object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }

        private StreamContent CreateStreamContent(Stream data)
        {
            var content= new StreamContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return content;
        }

    }
}