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

        public ContributorsDuringYearDTO ReadPerYear()
        {
            IQueryable<Contribuidor> itemsRead = dbContext.Contributors.Include("PeriodosContribucion").OrderBy(s => s.CuentaCotizacion);
            return BuildContributorsDuringYearDTO(itemsRead);
        }

        public ContributorsDuringYearDTO ReadPerYear(string healthCareId, bool showOnlyErrors=false)
        {
            IQueryable<Contribuidor> itemsRead = dbContext.Contributors.Include("PeriodosContribucion").OrderBy(s => s.CuentaCotizacion);

            if (!String.IsNullOrEmpty(healthCareId))
                itemsRead = itemsRead.Where(s => s.CuentaCotizacion.StartsWith(healthCareId));

            if (showOnlyErrors)
                itemsRead = itemsRead.Where(s => s.Valido == "False");

            return BuildContributorsDuringYearDTO(itemsRead);
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

        private ContributorsDuringYearDTO BuildContributorsDuringYearDTO(IQueryable<Contribuidor> items)
        {
            var result = new ContributorsDuringYearDTO();
            var yearsList = new List<string>();

            var partialResult = new List<Dictionary<string, string>>();
            foreach (Contribuidor contribuidor in items)
            {
                var data = new Dictionary<string, string>();
                data.Add("CuentaCotizacion", contribuidor.CuentaCotizacion.ToString());
                data.Add("Valido", contribuidor.Valido);

                foreach (PeriodoContribucion periodo in contribuidor.PeriodosContribucion)
                {
                    if (!data.Keys.Contains("HighResFileId"))
                        data.Add("HighResFileId", contribuidor.PeriodosContribucion.First().HighResImagenId);

                    if (!periodo.Valido)
                        data["Valido"] = "false";

                    data.Add(periodo.ComienzoPeriodo.Year.ToString(), periodo.Dinero.ToString());

                    
                    if (!yearsList.Contains(periodo.ComienzoPeriodo.Year.ToString()))
                        yearsList.Add(periodo.ComienzoPeriodo.Year.ToString());
                }
                partialResult.Add(data);
            }

            yearsList.Sort();
            result.YearsList = yearsList.ToArray();
            result.Data = partialResult;
            return result;

        }
    }
}