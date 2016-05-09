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
            return dbContext.Contributors.Select(s => s.IdentificadorSeguridadSocial).ToList();
        }

        public List<ContributionPeriodDTO> ReadContributorDetails(string healthCareId)
        {
            List<ContributionPeriodDTO> result = new List<ContributionPeriodDTO>();
            Contribuidor contributor = new Contribuidor();
            
            contributor = dbContext.Contributors.FirstOrDefault(s => s.IdentificadorSeguridadSocial == healthCareId);
                    
            if (contributor == null)
                return result;

            foreach (var contrPeriod in contributor.PeriodosContribucion)
            {
                var contrPeriodDTO = new ContributionPeriodDTO();
                contrPeriodDTO.healthCareId = healthCareId;
                contrPeriodDTO.MoneyContribution = contrPeriod.Dinero;
                contrPeriodDTO.PeriodStart = contrPeriod.ComienzoPeriodo;
                contrPeriodDTO.PeriodEnd = contrPeriod.FinPeriodo;
                contrPeriodDTO.HighResFileId = contrPeriod.HighResImagenId;
                result.Add(contrPeriodDTO);
            }

            return result;
        }

    }
}