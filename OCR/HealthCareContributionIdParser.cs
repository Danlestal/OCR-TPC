using System;
using System.Text.RegularExpressions;

namespace OCR
{
    public class ContributorPersonalData
    {

        public static Tuple<bool,string> ParseCotizationAccount(string text)
        {
            var idRegex = new Regex(Constants.IDRegExp);
            Match match = idRegex.Match(text);
            if (match.Success)
            {
                string result = match.Groups[1].Value.Trim();
                result = result.Replace(" ", "");
                // TODO: Añadido por Juan Luis porque confunde el 7 con la ?
                result = result.Replace('?', '7');
                result = result.Trim();

                if (!Regex.Match(result, @"\d{11}").Success)
                {
                    return new Tuple<bool, string>(false, result);
                }

                return new Tuple<bool, string>(true, result);
            }

            return new Tuple<bool, string>(false, "CC-NoEncontrada");
        }

        public static Tuple<bool, string> ParseSocialReason(string text)
        {
            var idRegex = new Regex(Constants.SocialReasonRegExp);
            Match match = idRegex.Match(text);
            if (match.Success)
            {
                return new Tuple<bool, string>(true, match.Groups[1].Value.Trim());
            }
            else
            {
                return new Tuple<bool, string>(false, "SocialReasonNotFound");
            }
        }

        public static Tuple<bool, string> ParseCNAE(string text)
        {
            var idRegex = new Regex(Constants.CNAERegExp);
            Match match = idRegex.Match(text);
            if (match.Success)
            {
                return new Tuple<bool, string>(true, match.Groups[1].Value.Trim());
            }
            else
            {
                return new Tuple<bool, string>(false, "CNAENotFound");
            }
        }

        public static Tuple<bool, string> ParseNIF(string text)
        {
            var idRegex = new Regex(Constants.NIFRegExp);
            Match match = idRegex.Match(text);
            if (match.Success)
            {
                return new Tuple<bool, string>(true, match.Groups[1].Value.Trim());
            }
            else
            {
                return new Tuple<bool, string>(false, "NIFNotFound");
            }
        }
    }
}