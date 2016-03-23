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
            Contributor contributor = dbContext.Contributors.FirstOrDefault(s => s.HealthCareContributorId == dataToInsert.ContributorId);
            if (contributor == null)
            {
                contributor = new Contributor();
                contributor.HealthCareContributorId = dataToInsert.ContributorId;
                dbContext.Contributors.Add(contributor);
            }

            foreach (ContributionPeriodDTO newPeriod in dataToInsert.ContributionPeriodsDTO)
            {
                contributor.Periods.Add(new ContributionPeriod()
                {
                    MoneyContribution = newPeriod.MoneyContribution,
                    PeriodEnd = newPeriod.PeriodEnd,
                    PeriodStart = newPeriod.PeriodStart
                });
            }

            dbContext.SaveChanges();
            return true;
            
        }
    }
}