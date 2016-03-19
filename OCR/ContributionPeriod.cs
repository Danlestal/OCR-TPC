using Newtonsoft.Json;
using System;

namespace OCR
{
    public class ContributionPeriod
    {

        public ContributionPeriod(DateTime periodStart, DateTime periodEnd, decimal moneyContribution)
        {
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            MoneyContribution = moneyContribution;
        }

        public DateTime PeriodStart { get; private set; }
        public DateTime PeriodEnd { get; private set; }
        public decimal MoneyContribution { get; private set; }
        
        /// <summary>
        /// Converts the object into a Json string.
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
