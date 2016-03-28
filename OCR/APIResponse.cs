using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OCR
{
    public class APIResponse
    {
        public APIResponse(HttpResponseMessage response)
        {
            Response = response;
        }

        public HttpResponseMessage Response { get; set; }

        internal async static Task<string> GetResult(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        public dynamic Data
        {
            get
            {
                var result = GetResult(Response);
                return Newtonsoft.Json.JsonConvert.DeserializeObject(result.Result);
            }
        }
    }
}