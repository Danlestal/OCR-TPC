using OCR_API.DataLayer;
using OCR_API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCR_API.InternalService
{
    public class ContributionPeriodInsertService
    {
        private OCR_TPC_Context dbContext;

        public ContributionPeriodInsertService(OCR_TPC_Context context)
        {
            dbContext = context;
        }


        public bool Insert(ContributionPeriodDataDTO dataToInsert)
        {
            Contribuidor contributor = dbContext.Contributors.FirstOrDefault(s => s.IdentificadorSeguridadSocial == dataToInsert.ContributorId);
            if (contributor == null)
            {
                contributor = new Contribuidor();
                contributor.IdentificadorSeguridadSocial = dataToInsert.ContributorId;
                dbContext.Contributors.Add(contributor);
            }

            foreach (ContributionPeriodDTO newPeriod in dataToInsert.ContributionPeriodsDTO)
            {
                var newContributionPeriod = new PeriodoContribucion()
                {
                    Dinero = newPeriod.MoneyContribution,
                    FinPeriodo = newPeriod.PeriodEnd,
                    ComienzoPeriodo = newPeriod.PeriodStart,
                    HighResImagenId = newPeriod.HighResFileId
                };

                dbContext.Periods.Add(newContributionPeriod);

                contributor.AddContributionPeriod(newContributionPeriod);
            }

            dbContext.SaveChanges();
            return true;
        }
    }
}