using OCR_API.DataLayer;
using OCR_API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCR_API.InternalService
{
    public class ContributorReadService
    {
        private OCR_TPC_Context dbContext;

        public ContributorReadService(OCR_TPC_Context context)
        {
            dbContext = context;
        }

        public List<string> ReadContributorsIds()
        {
            return dbContext.Contributors.Select(s => s.HealthCareContributorId).ToList();
        }

        public List<ContributionPeriodDTO> ReadContributorDetails(string healthCareId)
        {
            List<ContributionPeriodDTO> result = new List<ContributionPeriodDTO>();
            Contributor contributor = new Contributor();
            
            contributor = dbContext.Contributors.FirstOrDefault(s => s.HealthCareContributorId == healthCareId);
                    
            if (contributor == null)
                return result;

            foreach (var contrPeriod in contributor.ContributionPeriods)
            {
                var contrPeriodDTO = new ContributionPeriodDTO();
                contrPeriodDTO.MoneyContribution = contrPeriod.MoneyContribution;
                contrPeriodDTO.PeriodStart = contrPeriod.PeriodStart;
                contrPeriodDTO.PeriodEnd = contrPeriod.PeriodEnd;
                contrPeriodDTO.HighResFileId = contrPeriod.HighResFileId;
                result.Add(contrPeriodDTO);
            }

            return result;
        }

    }
}