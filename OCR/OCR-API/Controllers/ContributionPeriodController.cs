using OCR_API.DataLayer;
using OCR_API.DTOs;
using OCR_API.Filters;
using OCR_API.InternalService;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace OCR_API.Controllers
{
    public class ContributionPeriodController : ApiController
    {

        private ContributionPeriodInsertService insertionService;
        private ContributorReadService readService;

        public ContributionPeriodController()
        {
            var context = new OCR_TPC_Context();
            insertionService = new ContributionPeriodInsertService(context);
            readService = new ContributorReadService(context);
        }

        // POST: api/ContributionPeriod
        [Route("ContributionPeriods")]
        [ApiAuthenticationFilter(true)]
        public void Post(ContributionPeriodDataDTO value)
        {
           insertionService.Insert(value);
        }


        /// <summary>
        /// Gets the contribution periods
        /// </summary>
        /// <param name="healthCareid">The health careid.</param>
        /// <returns></returns>
        [HttpGet]
        [ApiAuthenticationFilter(true)]
        [Route("ContributionPeriods/{healthCareid}", Name = "ContributorDetails")]
        [ResponseType(typeof(IEnumerable<ContributionPeriodDTO>))]
        public IHttpActionResult Get(string healthCareid)
        {
            return Ok(readService.ReadContributorDetails(healthCareid));
        }
    }
}
