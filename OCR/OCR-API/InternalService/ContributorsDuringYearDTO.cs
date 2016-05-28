using System.Collections.Generic;

namespace OCR_API.InternalService
{
    public class ContributorsDuringYearDTO
    {
        public string[] YearsList { get; set; }
        public List<Dictionary<string, string>> Data { get; set; }
    }
}