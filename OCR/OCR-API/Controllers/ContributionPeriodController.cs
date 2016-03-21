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
        }
    }
}
