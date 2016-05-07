using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OCR
{
    public class LaboralLifeParser
    {

        public void ParseLaboralLife(string laboralLifeFilePathDocument)
        {
        }


        public void ParseTable(string tableText)
        {

            string[] tableLines = tableText.Split('\n');


            int startingIndex = GetTableStartLine(tableLines);
            int endIndex = GetTableEndLine(tableLines);

            List<LaboralLifeRow> rows = new List<LaboralLifeRow>();
            for( int i = startingIndex; i < endIndex; i++)
            {
                LaboralLifeRow parsedRow = ParseTableRow(tableLines[i]);
                if (parsedRow == null)
                {
                    rows.Last().Company += tableLines[i];
                }
                else
                {
                    rows.Add(parsedRow);
                }
            }
        }

        private int GetTableStartLine(string[] tableLines)
        {
            for (int i = 0; i < tableLines.Length; i++)
            {
                if (tableLines[i].StartsWith("REGIMEN"))
                {
                    return i + 1;
                }
            }

            throw new TableStartNotFoundException(); 
        }


        private int GetTableEndLine(string[] tableLines)
        {
            for (int i = 0; i < tableLines.Length; i++)
            {
                if (tableLines[i].StartsWith("REFERENCIAS ELECTRONICAS"))
                {
                    return i + 1;
                }
            }

            throw new TableStartNotFoundException();
        }

        public LaboralLifeRow ParseTableRow(string text)
        {
            Regex rowRegex = new Regex(@"(\S*) (\S*) (\D+)(\d\d.\d\d.\d\d\d\d) (\d\d.\d\d.\d\d\d\d) (.*)");
            Match matchResult = rowRegex.Match(text);
            if (!matchResult.Success)
                return null;

            LaboralLifeRow row = new LaboralLifeRow();
            row.Regimen = matchResult.Groups[1].Value.ToString();
            row.Code = matchResult.Groups[2].Value.ToString();
            row.Company = matchResult.Groups[3].Value.ToString();
            row.StartDate = matchResult.Groups[4].Value.ToString();
            row.EffectiveStartDate = matchResult.Groups[5].Value.ToString();
           
            string optionalPart = matchResult.Groups[6].Value.ToString();
            TryToMatchOptionalFields(row, optionalPart);

         
            return row;
        }

        private void TryToMatchOptionalFields(LaboralLifeRow row, string optionalPart)
        {

            Regex dateRegex = new Regex(@"(\d\d.\d\d.\d\d\d\d)");

            var dateMatch = dateRegex.Match(optionalPart);
            if (dateMatch.Success)
            {
                row.EndDate = dateMatch.Groups[1].Value.ToString();
                optionalPart = optionalPart.Replace(row.EndDate, "");
            }

            var tokens = optionalPart.Split(' ');
            row.Days = tokens.Last();

            if (tokens.Count() > 1)
            {
                row.GC = tokens[tokens.Count() - 2];
            }

            if (tokens.Count() > 2)
            {
                row.CT = tokens[tokens.Count() - 3];
            }
        }
    }

    public class LaboralLifeRow
    {
        public string Regimen { get; set; }
        public string Code { get; set; }
        public string Company { get; set; }
        public string StartDate { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EndDate { get; set; }
        public string CT { get; set; }
        public string CTP { get; set; }
        public string GC { get; set; }
        public string Days { get; set; }
    }
}
