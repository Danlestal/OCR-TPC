﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OCR
{
    public class ContributionPeriodsParser
    {
        public List<ContributionPeriod> Parse(string textToParse)
       {
            var result = new List<ContributionPeriod>();

            string[] lines = textToParse.Split('\n');

            int counter = 0;
            for (counter = 0; counter < lines.Length; ++counter)
            {
                if (lines[counter].StartsWith("Periodo Importe"))
                    break;
            }

            if (counter == lines.Length)
                return result;
            else
                counter++; //Skip the PeriodoImporte line.

            for (; counter < lines.Length; ++counter)
            {
                if (lines[counter].Trim() == string.Empty)
                    break;
                else
                {
                    result.Add(ParseContributionPeriod(lines[counter]));
                }
            }

            return result;
        }

        public ContributionPeriod ParseContributionPeriod(string textToParse)
        {
            //Enero 2011 a Diciembre 2011 99.920,66

            Regex contributionPeriodRegex = new Regex(Constants.PeriodRegExp);

            Match match = contributionPeriodRegex.Match(textToParse);
            if (!match.Success)
            {
                return null;
            }
            else
            {
                DateTime periodStart = new DateTime(Int32.Parse(match.Groups[2].Value), ParseSpanishMonth(match.Groups[1].Value), 1);
                DateTime periodEnd = new DateTime(Int32.Parse(match.Groups[4].Value), ParseSpanishMonth(match.Groups[3].Value), 1);
                double contribution = ParseMoneyOnSpanishFormat(match.Groups[5].Value);

                return new ContributionPeriod(periodStart, periodEnd, contribution);
            }
        }

        private double ParseMoneyOnSpanishFormat(string contributionString)
        {
            contributionString = contributionString.Replace(".", "");
            contributionString.Replace(",", ".");
            return double.Parse(contributionString);
        }
        public int ParseSpanishMonth(string value)
        {
            for (int i = 0; i < Constants.SpanishMonths.Length; ++i)
            {
                if(value.ToUpper().CompareTo(Constants.SpanishMonths[i].ToUpper())==0)
                {
                    return i + 1;
                }
            }

            throw new ArgumentException(value + " does not match any month");
        }
    }
}
