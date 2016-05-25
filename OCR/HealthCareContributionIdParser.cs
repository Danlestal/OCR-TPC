using System;
using System.Text.RegularExpressions;

namespace OCR
{
    public class ContributorPersonalData
    {

        public static string ParseCotizationAccount(string text)
        {
            var idRegex = new Regex(Constants.IDRegExp);
            Match match = idRegex.Match(text);
            if (match.Success)
            {
                string result = match.Groups[1].Value.Trim();
                result = result.Replace(" ", "");
                result = result.Trim();

                if (result.Length != 11)
                {
                    throw new ContributionPeriodCreationException("The text do not have the required format");
                }

                return result;
            }

            throw new ContributionPeriodCreationException("The text do not have the required format");
        }


        public static string ParseSocialReason(string text)
        {
            var idRegex = new Regex(Constants.SocialReasonRegExp);
            Match match = idRegex.Match(text);
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            throw new ArgumentException("The text do not have the required format");
        }

        public static string ParseCNAE(string text)
        {
            var idRegex = new Regex(Constants.CNAERegExp);
            Match match = idRegex.Match(text);
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            throw new ArgumentException("The text do not have the required format");
        }

        public static string ParseNIF(string text)
        {
            var idRegex = new Regex(Constants.NIFRegExp);
            Match match = idRegex.Match(text);
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            throw new ArgumentException("The text do not have the required format");
        }
    }
}