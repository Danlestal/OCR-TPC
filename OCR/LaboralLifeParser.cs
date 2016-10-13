using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace OCR
{
    public class LaboralLifeParser
    {

        public LaboralLifeData Parse(string file)
        {
            LaboralLifeData result = new LaboralLifeData();

            PDFToImageConverter converter = new PDFToImageConverter();
            OCR.OcrTextReader reader = new OCR.OcrTextReader();
            LaboralLifeParser parser = new LaboralLifeParser();

            List<PersonalData> personalDataList = new List<PersonalData>();
            string outputFileName = Path.GetFileNameWithoutExtension(file) + "-0.tiff";

            int numberOfPages = converter.GetPdfPages(file);
            if(numberOfPages == 0)
            {
                //NO pages on the pdf.
                return null;
            }

            //The personalData will always be on the first page
            converter.ConvertToImage(0, file, outputFileName, 512, 512, System.Drawing.Imaging.ImageFormat.Tiff);
            OcrData personalOCRData = null;
            try
            {
                personalOCRData = reader.Read(outputFileName);
            }
            catch (Exception)
            {
                return null;
            }

            PersonalData data = null;
            try
            {
                data = parser.ParsePersonalData(personalOCRData.Text, Path.GetFileName(file));
            }
            catch (Exception)
            {
                Console.Write("Problems parsing file " + file);
            }

            if (data != null)
                result.PersonalData = data;

            result.Rows = new List<LaboralLifeRow>();
            for(int i = 0; i < numberOfPages;i++)
            {
              
                outputFileName = Path.GetFileNameWithoutExtension(file) + "-" + i + ".tiff";
                converter.ConvertToImage(i, file, outputFileName, 512, 512, System.Drawing.Imaging.ImageFormat.Tiff);
                var tableOCRData = reader.Read(outputFileName);

                if ((tableOCRData != null) && (IsThereTable(tableOCRData.Text)))
                {
                    result.Rows.AddRange(parser.ParseTable(tableOCRData.Text));
                }
              
            }

            return result;
        }

        public PersonalData ParsePersonalData(string laboralLifeText, string fileName)
        {
            PersonalData data = new PersonalData();

            data.FileName = fileName;
            laboralLifeText = laboralLifeText.Replace('\n', ' ');

            Regex bornDate = new Regex(@"el (\d+ de .*?\d\d\d\d)");
            Match bornDateMatch = bornDate.Match(laboralLifeText);
            if(!bornDateMatch.Success)
            {
                data.BornDate = string.Empty;
            }
            else
            {
                data.BornDate = bornDateMatch.Groups[1].Value.ToString();
            }

            Regex name = new Regex(@"D[I\/]D.*? (.*?)[,.]");
            Match nameMatch = name.Match(laboralLifeText);
            if (!nameMatch.Success)
            {
                data.Name = string.Empty;
            }
            else
            {
                data.Name = nameMatch.Groups[1].Value.ToString();
            }

            Regex healthCareId = new Regex(Constants.NumSeguridadSocialRegExp);
            Match healthCareIdMatch = healthCareId.Match(laboralLifeText);
            if (!healthCareIdMatch.Success)
            {
                 data.HealthCareId = string.Empty;
            }
            else
            {
                data.HealthCareId = healthCareIdMatch.Groups[1].Value.ToString();
            }

            Regex dni = new Regex(Constants.DNIRegExp);
            Match matchDni = dni.Match(laboralLifeText);
            if (!matchDni.Success)
            {
                data.DNI = string.Empty;
            }
            else
            {
                data.DNI = matchDni.Groups[1].Value.ToString();
            }
           
            return data;
        }

        public bool IsThereTable(string text)
        {
            return (GetTableStartLine(text.Split('\n')) >= 0);
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
                    if (tableLines[i].Split(' ').Count() == 1)
                    {
                        if (rows.Count()>0)
                            rows.Last().Company += tableLines[i];
                    }
                        
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
                if ( (tableLines[i].StartsWith("REGIMEN")) || (tableLines[i].StartsWith("Régimen")) || (tableLines[i].StartsWith("Regimen")) || (tableLines[i].StartsWith("R'gimen")) || (tableLines[i].StartsWith("REG'MEN")))
                {
                    return i + 1;
                }
            }

            return -1;
        }

        private int GetTableEndLine(string[] tableLines)
        {
            for (int i = 0; i < tableLines.Length; i++)
            {
                if (tableLines[i].StartsWith("REFERENCIAS ELECTR") || (tableLines[i].Contains("Resumen de huellas")) || (tableLines[i].Contains("Se informa")))
                {
                    return i;
                }
            }

            throw new TableStartNotFoundException();
        }

        private LaboralLifeRow ParseTableRow(string text)
        {
            Regex rowRegex = new Regex(@"(\S*) (\S*) (\D+)(\d\d.\d\d.\d\d\d\d) (\d\d.\d\d.\d\d\d\d) (.*)");
            Match matchResult = rowRegex.Match(text);
            if (!matchResult.Success)
            {
                rowRegex = new Regex(@"(\S*) (\S*) (\D+)(\d\d.\d\d.\d\d) (\d\d.\d\d.\d\d\d\d) (.*)");
                matchResult = rowRegex.Match(text);
                if (!matchResult.Success)
                {
                    return null;
                }
            }
               
            // Si no hay Código de Cotización no grabo el registro ya que no sirve.
            if (string.IsNullOrEmpty(ProCode(matchResult.Groups[2].Value.ToString())))
                return null;
            // Si son vacaciones no grabo el registro ya que no sirve.
            if (ProCompany(matchResult.Groups[3].Value.ToString()).Contains("VACACIONES") || (ProCompany(matchResult.Groups[3].Value.ToString()).Contains("vacaciones")))
                return null;

            LaboralLifeRow row = new LaboralLifeRow();
            row.Regimen = matchResult.Groups[1].Value.ToString();
            row.Code = ProCode(matchResult.Groups[2].Value.ToString());
            row.Company = ProCompany(matchResult.Groups[3].Value.ToString());
            row.StartDate = matchResult.Groups[4].Value.ToString();
            row.EffectiveStartDate = matchResult.Groups[5].Value.ToString();
           
            string optionalPart = matchResult.Groups[6].Value.ToString();
            TryToMatchOptionalFields(row, optionalPart);

         
            return row;
        }

        private string ProCompany(string parsedCode)
        {
            string partial = parsedCode.Replace(".", "");
            partial = partial.Replace("-", "");
            partial = partial.Replace("_", "");
            partial = partial.Replace("—", "");
            return partial;
        }

        private string ProCode(string parsedCode)
        {
            string partial = parsedCode.Replace('o', '0');
            partial = partial.Replace('O', '0');

            double numericValue = 0;

            if (double.TryParse(partial, out numericValue))
            {
                return numericValue.ToString();
            }
            else
            {
                return string.Empty;
            }

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
            else
            {
                Regex auxDateRegex = new Regex(@"(\d\d.\d\d.\d\d)");
                dateMatch = auxDateRegex.Match(optionalPart);
                if (dateMatch.Success)
                {
                    row.EndDate = dateMatch.Groups[1].Value.ToString();
                    optionalPart = optionalPart.Replace(row.EndDate, "");
                }
            }


            var tokens = optionalPart.Split(' ');

            int numericResult = 0;

            if (Int32.TryParse(tokens.Last(), out numericResult))
            {
                row.Days = numericResult.ToString();
            }
            else
            {
                row.Days = string.Empty;
            }

            if (tokens.Count() > 1)
            {
                if (Int32.TryParse(tokens[tokens.Count() - 2], out numericResult))
                {
                    row.GC = numericResult.ToString();
                }
                else
                {
                    row.GC = string.Empty;
                }
            }

            if (tokens.Count() > 2)
            {
                if (Int32.TryParse(tokens[tokens.Count() - 3], out numericResult))
                {
                    row.CT = numericResult.ToString();
                }
                else
                {
                    row.CT = string.Empty;
                }
            }
        }
    }
}
