using OCR_API.DataLayer;
using OCR_API.DTOs;
using OCR_API.InternalService;
using System;
using System.Collections.Generic;
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

        //POST: api/Image
        public void Post(string file)
        {
            uploadService.uploadImage(file, "url");
        }

        // GET: api/Image
        //public ActionResult Index()
        //{
        //    return View();
        //}


    }
}
