using Newtonsoft.Json;
using System;

namespace OCR
{
    public class ContributionPeriod
    {

        public ContributionPeriod(DateTime periodStart, DateTime periodEnd, double moneyContribution, bool valid)
        {
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            MoneyContribution = moneyContribution;
            ValidPeriod = valid;
        }

        public DateTime PeriodStart { get; private set; }
        public DateTime PeriodEnd { get; private set; }
        public double MoneyContribution { get; private set; }
        public bool ValidPeriod { get; private set; }


        /// <summary>
        /// Converts the object into a Json string.
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override bool Equals(object obj)
        {
            ContributionPeriod target = obj as ContributionPeriod;
            if (target == null)
                return false;

            return ((target.PeriodStart == this.PeriodStart) && (target.PeriodEnd == this.PeriodEnd) && (target.MoneyContribution == MoneyContribution));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
