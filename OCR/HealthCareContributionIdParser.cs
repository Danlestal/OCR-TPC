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
                result = result.Replace(']', '1');
                result = result.Trim();

                if (!Regex.Match(result, @"\d{11}").Success)
                {
                    return new Tuple<bool, string>(false, result);
                }

                return new Tuple<bool, string>(true, result);
            }

            throw new ArgumentException("text", "The text can not be parsed");
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
                // SOMETIMES THE CNAE GETS MISPLACED TO THE BEGINNING OF THE LINE
                var idRegex2 = new Regex(@"(^\d+)", RegexOptions.Multiline);
                Match match2 = idRegex2.Match(text);
                if (match2.Success)
                {
                    return new Tuple<bool, string>(true, match2.Groups[1].Value.Trim());
                }

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
                // SOMETIMES THE CNAE GETS MISPLACED TO THE BEGINNING OF THE LINE
                var idRegex2 = new Regex(@"(^\d.\d\d\d\d\d\d\d\d)", RegexOptions.Multiline);
                Match match2 = idRegex2.Match(text);
                if (match2.Success)
                {

                    string possibleNIF = match2.Groups[1].Value.Trim();
                    var auxChar = possibleNIF.ToCharArray();
                    if (auxChar[1] == '8')
                    {
                        auxChar[1] = 'B';
                    }

                    return new Tuple<bool, string>(true, new string(auxChar));
                }

                


                return new Tuple<bool, string>(false, "NIFNotFound");
            }
        }
    }
}