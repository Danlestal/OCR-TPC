using OCR_API.DataLayer;
using OCR_API.DTOs;
using OCR_API.Filters;
using OCR_API.InternalService;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OCR_API.Controllers
{
    [EnableCors("*", "*", "*")]
    public class LaboralLifeController : ApiController
    {

        private LaboralLifeInsertService insertionService;

        public LaboralLifeController()
        {
            var context = new OCR_TPC_Context();
            insertionService = new LaboralLifeInsertService(context);
        }

        // POST: api/ContributionPeriod
        [Route("LaboralLife")]
        //[ApiAuthenticationFilter(true)]
        public void Post(LaboralLifeDTO value)
        {
            insertionService.Insert(value);
        }

    }
}