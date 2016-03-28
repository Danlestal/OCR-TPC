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

        public ICollection<ContributionPeriod> Periods { get; set; }

        public void AddContributionPeriod(ContributionPeriod newPeriod)
        {
           if (!Periods.Contains(newPeriod))
            {
                Periods.Add(newPeriod);
            }

        }
    }
}