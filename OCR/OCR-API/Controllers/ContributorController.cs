using OCR_API.DataLayer;
using OCR_API.DTOs;
using OCR_API.InternalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace OCR_API.Controllers
{
    public class ContributorController : ApiController
    {
        private ContributorReadService readService;

        public ContributorController()
        {
            var context = new OCR_TPC_Context();
            readService = new ContributorReadService(context);
        }


        /// <summary>
        /// Gets all the health care ids of our contributors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Contributors")]
        [ResponseType(typeof(IEnumerable<string>))]
        public IHttpActionResult Get()
        {
            return Ok(readService.ReadContributorsIds());
        }
    }
}
