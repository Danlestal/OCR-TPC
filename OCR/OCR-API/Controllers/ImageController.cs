using OCR_API.DataLayer;
using OCR_API.DTOs;
using OCR_API.InternalService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OCR_API.Controllers
{
    public class ImageController : ApiController
    {

        private ImageUploadService uploadService;
        //private ImageReadService readService;

        public ImageController()
        {
            //var context = new OCR_TPC_Context();
            uploadService = new ImageUploadService();
        }


        [HttpPost]
        [Route("Image")]
        public IHttpActionResult Post(Stream fileStream)
        {
            if (!Directory.Exists(ConfigurationManager.AppSettings["StorageSourceFolder"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["StorageSourceFolder"]);
            }

            string newFileName = Path.GetRandomFileName();
            string filePath = ConfigurationManager.AppSettings["StorageSourceFolder"]  + "\\" + newFileName;
            using (var output = File.Create(filePath))
            {
                fileStream.CopyTo(output);
            }
            return Ok(newFileName);
        }

        //POST: api/Image
        /*
        public void Post(string file, string url)
        {
            uploadService.UploadImage(file, url);
        }*/

        // GET: api/Image
        //public ActionResult Index()
        //{
        //    return View();
        //}


    }
}
