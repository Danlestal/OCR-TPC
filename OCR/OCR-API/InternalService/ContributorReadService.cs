using OCR_API.DataLayer;
using OCR_API.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            return dbContext.Contributors.Select(s => s.CuentaCotizacion).ToList();
        }

        public List<ContributionPeriodDTO> ReadWithLimit()
        {
            int queryLimit = int.Parse(ConfigurationManager.AppSettings["QueryLimit"]);

            List<PeriodoContribucion> periods = new List<PeriodoContribucion>();
            List<ContributionPeriodDTO> result = new List<ContributionPeriodDTO>();

            periods = dbContext.Periods.Take(queryLimit).ToList();

            foreach (var contrPeriod in periods)
            {
                var contrPeriodDTO = new ContributionPeriodDTO();
                contrPeriodDTO.HealthCareId = contrPeriod.Contributor.CuentaCotizacion;
                contrPeriodDTO.MoneyContribution = contrPeriod.Dinero;
                contrPeriodDTO.PeriodStart = contrPeriod.ComienzoPeriodo;
                contrPeriodDTO.PeriodEnd = contrPeriod.FinPeriodo;
                contrPeriodDTO.HighResFileId = contrPeriod.HighResImagenId;
                result.Add(contrPeriodDTO);
            }
            return result;
        }

        internal void DeleteByHealthCareId(string healthcareid)
        {
            Contribuidor toRemove = dbContext.Contributors.FirstOrDefault(s => s.CuentaCotizacion == healthcareid);
            dbContext.Contributors.Remove(toRemove);
            dbContext.SaveChanges();
        }

        public List<ContributionPeriodDTO> ReadContributorDetails(string healthCareId)
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