using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCR_API.DataLayer
{
    public class Contributor
    {
        public Contributor()
        {
            Periods = new List<ContributionPeriod>();
        }

        public int id { get; set; }

        public string HealthCareContributorId { get; set; }

        public List<ContributionPeriod> Periods { get; set; }
    }
}