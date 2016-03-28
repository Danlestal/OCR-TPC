using System;

namespace OCR_API.DataLayer
{
    public class ContributionPeriod
    {

        public int id { get; set; }

        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }

        public double MoneyContribution { get; set; }


        public override bool Equals(object obj)
        {
            ContributionPeriod target = obj as ContributionPeriod;
            return ((target.PeriodEnd == PeriodEnd) && (target.PeriodStart == PeriodStart) && (target.MoneyContribution == MoneyContribution));
        }


    }
}