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
                    ContributionPeriod parsedPeriod = ParseContributionPeriod(lines[counter]);
                    if (parsedPeriod != null)
                        result.Add(parsedPeriod);
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

                if (periodStart > periodEnd)
                {
                    throw new ContributionPeriodCreationException();
                }


                double contribution = ParseMoneyOnSpanishFormat(match.Groups[5].Value);
               


                return new ContributionPeriod(periodStart, periodEnd, contribution);
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
