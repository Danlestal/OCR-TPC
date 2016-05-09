using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCR_API.DTOs
{
    public class ContributionPeriodDataDTO
    {

        public double ContributorId { get; set; }
        public string SocialReason { get; set; }
        public string CNAE { get; set; }
        public List<ContributionPeriodDTO> ContributionPeriodsDTO { get; set; }
       
    }

    public class ContributionPeriodDTO
    {
        public double HealthCareId { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public double MoneyContribution { get; set; }
        public string HighResFileId { get; set; }
    }
}