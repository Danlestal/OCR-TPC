using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OCR_API.DataLayer
{
    public class ContributionPeriod
    {

        public int ContributionPeriodId { get; set; }

        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }

        public double MoneyContribution { get; set; }

        public int ContributorRefId { get; set; }

        public string HighResFileId { get; set; }

        [ForeignKey("ContributorRefId")]
        public virtual Contributor Contributor { get; set; }

       
    }
}