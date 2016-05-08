using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCR_API.DTOs
{
    public class ContributionPeriodDataDTO
    {

        public string ContributorId { get; set; }
        public List<ContributionPeriodDTO> ContributionPeriodsDTO { get; set; }
       
    }

    public class ContributionPeriodDTO
    {
        public string healthCareId { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public double MoneyContribution { get; set; }
        public string HighResFileId { get; set; }
    }
}