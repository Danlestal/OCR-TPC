using System.Collections.Generic;

namespace OCR_API.InternalService
{
    public class ContributionPerioPerYeardDTO
    {
        public ContributionPerioPerYeardDTO()
        {
            Data = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Data { get; set; }
    }
}