using System;
using Microsoft.Web.Administration;
using OCR_API.Filters;
using OCR_API.InternalService;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Linq;
using System.Web;

namespace OCR_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImageController : ApiController
    {

        private ImageUploadService uploadService;
        private string baseImageUrl;
        private string imageStoragePath;

        public ImageController()
        {
            uploadService = new ImageUploadService();
            baseImageUrl = @"http:\\" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "\\" + ConfigurationManager.AppSettings["StorageSourceFolder"];
            imageStoragePath = HttpContext.Current.Server.MapPath(".") + "\\"+ ConfigurationManager.AppSettings["StorageSourceFolder"];
        }

        [HttpPost]
        [Route("UploadFile")]
        [ApiAuthenticationFilter(true)]
        public HttpResponseMessage Post()
        {
            var task = this.Request.Content.ReadAsStreamAsync();
            task.Wait();
            Stream requestStream = task.Result;

            string newFileName = Path.GetRandomFileName();
            newFileName = Path.GetFileNameWithoutExtension(newFileName) + ".png";

            try
            {
                if (!Directory.Exists(imageStoragePath))
                {
                    Directory.CreateDirectory(imageStoragePath);
                }

                Stream fileStream = File.Create(imageStoragePath + "\\" + newFileName);
                requestStream.CopyTo(fileStream);
                fileStream.Close();
                requestStream.Close();
            }
            catch (Exception a)
            {
                File.WriteAllText("lol.txt", a.Message + "\n " + a.InnerException.Message);
            }
            


            string fullpath = Path.GetFullPath(imageStoragePath + "\\" + newFileName);

            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Created;
            response.Content = new StringContent(baseImageUrl + "\\" + newFileName + "@" + fullpath);
            return response;
        }

       

    }
}
