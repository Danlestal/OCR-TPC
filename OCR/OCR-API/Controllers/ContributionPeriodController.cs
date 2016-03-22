using Newtonsoft.Json;
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
    public class ContributionPeriodController : ApiController
    {

        private ContributionPeriodInsertService insertionService;


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
        public void Post([FromBody]string value)
        {
            var dataToInsert = JsonConvert.DeserializeObject<ContributionPeriodDataDTO>(value);
            insertionService.Insert(dataToInsert);
        }
    }
}
