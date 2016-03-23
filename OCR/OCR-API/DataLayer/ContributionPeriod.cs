﻿using System;

namespace OCR_API.DataLayer
{
    public class ContributionPeriod
    {

        public int id { get; set; }

        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }

        public double MoneyContribution { get; set; }
    }
}