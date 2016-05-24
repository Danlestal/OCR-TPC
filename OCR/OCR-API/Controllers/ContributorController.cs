using OCR_API.DataLayer;
using OCR_API.Filters;
using OCR_API.InternalService;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace OCR_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ContributorController : ApiController
    {
        private ContributorReadService contributorService;
        private OCR_TPC_Context context;

        public ContributorController()
        {
            context = new OCR_TPC_Context();
            contributorService = new ContributorReadService(context);
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
            return Ok(contributorService.ReadContributorsIds());
        }

        [HttpDelete]
        [Route("Contributors/{id}")]
        [ApiAuthenticationFilter(true)]
        public void Delete(int id)
        {
            contributorService.Delete(id);
        }



    }
}
