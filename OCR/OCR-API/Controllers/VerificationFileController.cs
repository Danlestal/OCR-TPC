using NPOI.HSSF.UserModel;
using OCR_API.DataLayer;
using OCR_API.Filters;
using OCR_API.InternalService;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OCR_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VerificationFileController : ApiController
    {
        private VerificationServices verifyService;
        private OCR_TPC_Context context;
        private string xlsStoragePath;

        public VerificationFileController()
        {
            context = new OCR_TPC_Context();
            verifyService = new VerificationServices(context);
            xlsStoragePath = HttpContext.Current.Server.MapPath(".") + "\\" + ConfigurationManager.AppSettings["StorageSourceFolder"];
        }

        // POST: api/ContributionPeriod
        [Route("VerificationFile")]
        [ApiAuthenticationFilter(true)]
        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            string verificationResult = string.Empty;
            // Check if files are available
            if (httpRequest.Files.Count > 0)
            {
                var files = new List<string>();
                
                // interate the files and save on the server
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = xlsStoragePath + postedFile.FileName;
                    postedFile.SaveAs(filePath);

                    files.Add(filePath);
                    verificationResult += verifyService.FileVerification(filePath);
                }
                
            }
            else
            {
                // return BadRequest (no file(s) available)
                return result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Created;
            response.Content = new StringContent(verificationResult);

            return response;
        }
    }
}
