﻿using OCR_API.DataLayer;
using OCR_API.Filters;
using OCR_API.InternalService;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace OCR_API.Controllers
{
    public class ContributorController : ApiController
    {
        private ContributorReadService readService;
        private OCR_TPC_Context context;

        public ContributorController()
        {
            context = new OCR_TPC_Context();
            readService = new ContributorReadService(context);
        }


        /// <summary>
        /// Gets all the health care ids of our contributors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Contributors")]
        [ApiAuthenticationFilter(true)]
        [ResponseType(typeof(IEnumerable<string>))]
        public IHttpActionResult Get()
        {
            return Ok(readService.ReadContributorsIds());
        }
    }
}
