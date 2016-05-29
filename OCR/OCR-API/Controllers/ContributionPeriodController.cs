using OCR_API.DataLayer;
using OCR_API.DTOs;
using OCR_API.Filters;
using OCR_API.InternalService;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace OCR_API.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ContributionPeriodController : ApiController
    {

        private ContributionPeriodInsertService periodsService;
        private ContributorReadService contributorsService;

        public ContributionPeriodController()
        {
            var context = new OCR_TPC_Context();
            periodsService = new ContributionPeriodInsertService(context);
            contributorsService = new ContributorReadService(context);
        }

        // POST: api/ContributionPeriod
        [Route("ContributionPeriods")]
        [ApiAuthenticationFilter(true)]
        public void Post(ContributionPeriodDataDTO value)
        {
            periodsService.Insert(value);
        }

       

        [HttpGet]
        [ApiAuthenticationFilter(true)]
        [Route("ContributionPeriods")]
        [ResponseType(typeof(IEnumerable<ContributionPeriodDTO>))]
        public IHttpActionResult GetFixed()
        {
            return Ok(contributorsService.ReadWithLimit());
        }


        [HttpGet]
        [ApiAuthenticationFilter(true)]
        [Route("ContributionPeriods/PerYear")]
        [ResponseType(typeof(IEnumerable<ContributionPeriodDTO>))]
        public IHttpActionResult GetContributionPeriodsPerYear()
        {
            return Ok(contributorsService.ReadPerYear());
        }


        [HttpGet]
        [ApiAuthenticationFilter(true)]
        [Route("ContributionPeriods/PerYear/{healthCareid}")]
        [ResponseType(typeof(IEnumerable<ContributionPeriodDTO>))]
        public IHttpActionResult GetSpecificContributionPeriodsPerYear(string healthCareId, bool showOnlyErrors = false)
        {
            return Ok(contributorsService.ReadPerYear(healthCareId, showOnlyErrors));
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
            return Ok(contributorsService.ReadContributorDetails(healthCareid));
        }

        /// <summary>
        /// Gets the contribution periods
        /// </summary>
        /// <param name="healthCareidList">The health careid.</param>
        /// <returns></returns>
        [HttpGet]
        [ApiAuthenticationFilter(true)]
        [Route("ContributionPeriodsList/{healthCareidList}", Name = "ContributorsDetails")]
        [ResponseType(typeof(IEnumerable<ContributionPeriodDTO>))]
        public IHttpActionResult GetList(string[] healthCareidList)
        {
            List<ContributionPeriodDTO> result = new List<ContributionPeriodDTO>();
            foreach (string id in healthCareidList)
            {
                result.AddRange(contributorsService.ReadContributorDetails(id));
            }
            return Ok(result);
        }
    }
}