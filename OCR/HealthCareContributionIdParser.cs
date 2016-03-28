using System;
using System.Text.RegularExpressions;

namespace OCR
{
    public class HealthCareContributionIdParser
    {

        public static string Parse(string text)
        {
            var idRegex = new Regex(Constants.IDRegExp);
            Match match = idRegex.Match(text);
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            throw new ArgumentException("The text do not have the required format");
        }

    }
}