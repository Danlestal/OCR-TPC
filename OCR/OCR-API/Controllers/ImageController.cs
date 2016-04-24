using OCR_API.Filters;
using OCR_API.InternalService;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OCR_API.Controllers
{
    public class ImageController : ApiController
    {

        private ImageUploadService uploadService;

        public ImageController()
        {
            uploadService = new ImageUploadService();
        }

        [HttpPost]
        [Route("UploadFile")]
        [ApiAuthenticationFilter(true)]
        public HttpResponseMessage Post()
        {
            var task = this.Request.Content.ReadAsStreamAsync();
            task.Wait();
            Stream requestStream = task.Result;

            if (!Directory.Exists(ConfigurationManager.AppSettings["StorageSourceFolder"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["StorageSourceFolder"]);
            }

            string newFileName = Path.GetRandomFileName();
            newFileName = Path.GetFileNameWithoutExtension(newFileName) + ".tiff";

            Stream fileStream = File.Create(ConfigurationManager.AppSettings["StorageSourceFolder"] + "\\"+ newFileName);
            requestStream.CopyTo(fileStream);
            fileStream.Close();
            requestStream.Close();
            

            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Created;
            response.Content = new StringContent(newFileName);
            return response;
        }

    }
}
