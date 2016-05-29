using System;
using System.Collections.Generic;

namespace OCR
{

    public class ContributionPeriodDataDTO
    {

        public ContributionPeriodDataDTO()
        {
            ContributionPeriodsDTO = new List<ContributionPeriodDTO>();
            Valid = true;
        }

        public string ContributorId { get; set; }
        public string SocialReason { get; set; }
        public string CNAE { get; set; }
        public string NIF { get; set; }
        public string PathAbsoluto { get; set; }
        public bool Valid { get; set; }

        public List<ContributionPeriodDTO> ContributionPeriodsDTO { get; set; }
        
    }

    public class ContributionPeriodDTO
    {
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public double MoneyContribution { get; set; }
        public string HighResFileId { get; set; }
        public string FileAbsolutePath { get; set; }
        public bool Valid { get; set; }
    }
}
