using System;
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
                if (lines[counter].StartsWith("Periodo Importe") || lines[counter].StartsWith("Período Importe"))
                    break;
            }

            if (counter == lines.Length)
                return result;
            else
                counter++; //Skip the PeriodoImporte line.

            for (; counter < lines.Length; ++counter)
            {
                if (lines[counter].Trim() == string.Empty)
                    continue;
                else
                {
                    ContributionPeriod parsedPeriod = ParseContributionPeriod(lines[counter]);
                    if (parsedPeriod != null)
                        result.Add(parsedPeriod);
                }
            }

            return result;
        }

        public ContributionPeriod ParseContributionPeriod(string textToParse)
        {
            bool validPeriod = true;

            Regex contributionPeriodRegex = new Regex(Constants.PeriodRegExp);

            Match match = contributionPeriodRegex.Match(textToParse);
            if (!match.Success)
            {
                return null;
            }
            else
            {
                DateTime periodStart;
                DateTime periodEnd;
                try
                {
                    periodStart = new DateTime(Int32.Parse(match.Groups[2].Value), ParseSpanishMonth(match.Groups[1].Value), 1);
                    periodEnd = new DateTime(Int32.Parse(match.Groups[4].Value), ParseSpanishMonth(match.Groups[3].Value), 1);
                }
                catch(Exception)
                {
                    return null;
                }

                if (periodStart > periodEnd)
                {
                    validPeriod = false;
                }

                if (periodStart.Year != periodEnd.Year)
                {
                    validPeriod = false;
                }


                double contribution = ParseMoneyOnSpanishFormat(match.Groups[5].Value);
                return new ContributionPeriod(periodStart, periodEnd, contribution, validPeriod);
            }
        }

        private double ParseMoneyOnSpanishFormat(string contributionString)
        {
            contributionString = contributionString.Replace(".", "");
            Regex moneyRegex = new Regex(@"(\d+[,.]*\d*)");
            Match matchResult = moneyRegex.Match(contributionString);

            if (!matchResult.Success)
            {
                return 0;
            }
            
            contributionString = matchResult.Groups[1].Value.ToString();

            double result = 0;

            double.TryParse(contributionString, out result);
            return result;
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
