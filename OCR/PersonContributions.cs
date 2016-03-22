using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR
{
    public class PersonContributions
    {
        public string HealthCareContributionId { get; private set; }
        public List<ContributionPeriod> Contributions { get; private set; }


        public PersonContributions(string healthCareContributionId, List<ContributionPeriod> contributions)
        {
            HealthCareContributionId = healthCareContributionId;
            Contributions = contributions;
        }
    }
}
