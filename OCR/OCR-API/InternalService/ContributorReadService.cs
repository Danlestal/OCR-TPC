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

        public List<double> ReadContributorsIds()
        {
            return dbContext.Contributors.Select(s => s.CuentaCotizacion).ToList();
        }

        public List<ContributionPeriodDTO> ReadContributorDetails(double healthCareId)
        {
            List<ContributionPeriodDTO> result = new List<ContributionPeriodDTO>();
            Contribuidor contributor = new Contribuidor();
            
            contributor = dbContext.Contributors.FirstOrDefault(s => s.CuentaCotizacion == healthCareId);
                    
            if (contributor == null)
                return result;

            foreach (var contrPeriod in contributor.PeriodosContribucion)
            {
                var contrPeriodDTO = new ContributionPeriodDTO();
                contrPeriodDTO.HealthCareId = healthCareId;
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