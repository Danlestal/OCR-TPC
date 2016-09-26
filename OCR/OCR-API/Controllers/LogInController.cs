using OCR_API.DataLayer;
using OCR_API.InternalService;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OCR_API.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LogInController : ApiController
    {
        private BusinessServices businessService;
        private OCR_TPC_Context context;

        public LogInController()
        {
            context = new OCR_TPC_Context();
            businessService = new BusinessServices(context);
        }

        // POST: api/ContributionPeriod
        [HttpPost]
        [Route("LogIn")]
        public HttpResponseMessage Post()
        {
            var httpRequest = HttpContext.Current.Request;
            string user = httpRequest.Form["user"];
            string password = httpRequest.Form["password"];

            HttpResponseMessage response = new HttpResponseMessage();

            string credentials = businessService.LogIn(user, password);
            
            if (credentials != string.Empty)
            {
                response.StatusCode = HttpStatusCode.Created;
                response.Content = new StringContent(credentials);
                return response;
            }

            response.StatusCode = HttpStatusCode.BadRequest;
            
            return response;
        }
    }
}
