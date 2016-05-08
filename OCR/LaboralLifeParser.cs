﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OCR
{
    public class LaboralLifeParser
    {

        public PersonalData ParsePersonalData(string laboralLifeText)
        {
            PersonalData data = new PersonalData();

            laboralLifeText = laboralLifeText.Replace('\n', ' ');

            Regex bornDate = new Regex(@"el (\d+ de .*?\d\d\d\d)");
            Match bornDateMatch = bornDate.Match(laboralLifeText);
            if(!bornDateMatch.Success)
            {
                data.BornDate = string.Empty;
            }

            Regex name = new Regex(@"D[I\/]D.*? (.*?)[,.]");
            Match nameMatch = name.Match(laboralLifeText);
            if (!nameMatch.Success)
            {
                throw new WrongPersonalTextException();
            }

            Regex healthCareId = new Regex(@"mero de la Seguridad Social (.*?)[,\.]");
            Match healthCareIdMatch = healthCareId.Match(laboralLifeText);
            if (!healthCareIdMatch.Success)
            {
                healthCareId = new Regex(@"mero dela Seguridad Social (.*?)[,\.]");
                healthCareIdMatch = healthCareId.Match(laboralLifeText);
                if (!healthCareIdMatch.Success)
                {
                    data.HealthCareId = string.Empty;
                }
            }

            Regex dni = new Regex(@"(\d\d\d\d\d\d\d\d\d[A-Z])");
            Match matchDni = dni.Match(laboralLifeText);
            if (!matchDni.Success)
            {
                throw new WrongPersonalTextException();
            }
           
            data.Name = nameMatch.Groups[1].Value.ToString();
            data.BornDate = bornDateMatch.Groups[1].Value.ToString();
            data.HealthCareId = healthCareIdMatch.Groups[1].Value.ToString();
            data.DNI = matchDni.Groups[1].Value.ToString();

            return data;
        }

        public List<LaboralLifeRow> ParseTable(string tableText)
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

            return rows;
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

        private LaboralLifeRow ParseTableRow(string text)
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
}
