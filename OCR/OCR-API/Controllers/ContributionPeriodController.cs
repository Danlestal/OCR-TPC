using OCR_API.DataLayer;
using OCR_API.DTOs;
using OCR_API.InternalService;
using System.Collections.Generic;
using System.Web.Http;

namespace OCR_API.Controllers
{
    public class ContributionPeriodController : ApiController
    {

        private ContributionPeriodInsertService insertionService;

        public ContributionPeriodController()
        {
            var context = new OCR_TPC_Context();
            insertionService = new ContributionPeriodInsertService(context);
        }



        // GET: api/ContributionPeriod
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ContributionPeriod/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ContributionPeriod
        public void Post(ContributionPeriodDataDTO value)
        {
           insertionService.Insert(value);
        }
    }
}
