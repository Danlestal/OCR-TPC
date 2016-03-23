﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR
{

    public class ContributionPeriodDataDTO
    {

        public ContributionPeriodDataDTO()
        {
            ContributionPeriodsDTO = new List<ContributionPeriodDTO>();
        }

        public string ContributorId { get; set; }

        public List<ContributionPeriodDTO> ContributionPeriodsDTO { get; set; }


    }

    public class ContributionPeriodDTO
    {
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public double MoneyContribution { get; set; }
    }
}