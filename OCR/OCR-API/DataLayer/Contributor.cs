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
            ContributionPeriods = new List<ContributionPeriod>();
        }

        public int ContributorId { get; set; }

        public string HealthCareContributorId { get; set; }

        public virtual ICollection<ContributionPeriod> ContributionPeriods { get; set; }

        public void AddContributionPeriod(ContributionPeriod newPeriod)
        {
           if (!ContributionPeriods.Contains(newPeriod))
            {
                ContributionPeriods.Add(newPeriod);
            }

        }
    }
}