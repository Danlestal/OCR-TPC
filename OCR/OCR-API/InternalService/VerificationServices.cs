using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OCR_API.DataLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OCR_API.InternalService
{
    public class VerificationServices
    {

        private OCR_TPC_Context dbContext;

        public VerificationServices(OCR_TPC_Context context)
        {
            dbContext = context;

        }


        public string FileVerification(string filePath)
        {
            Contribuidor contributor = new Contribuidor();
            string result = "";

            FileStream _fileStream = new FileStream(filePath, FileMode.Open,
                                      FileAccess.Read);
            
            IWorkbook _workbook = WorkbookFactory.Create(_fileStream);
            _fileStream.Close();

            int sheetsNumber = _workbook.NumberOfSheets;

            for (int i = 0; i < sheetsNumber; i++)
            {
                ISheet _worksheet = _workbook.GetSheetAt(i);
                string sheetName = _worksheet.SheetName;
                
                contributor = dbContext.Contributors.FirstOrDefault(s => s.IdentificadorSeguridadSocial == sheetName);
                if (contributor == null)
                {
                    result += "<p>La Cta. Seguridad Social " + sheetName + " no se encontró en la Base de datos.Verifique el nombre de la pestaña<p>";
                } else
                {
                    bool verify = true;
                    string rowResult = string.Empty;

                    for (int row = 1; row <= _worksheet.LastRowNum; row++)
                    {
                        
                        IRow eachRow = _worksheet.GetRow(row);
                        if (eachRow == null) //null is when the row only contains empty cells 
                        {
                            continue;
                        }
                        if (eachRow.GetCell(0) == null)
                        {
                            break;
                        }
                        bool periodExist = false;
                        foreach (var contrPeriod in contributor.PeriodosContribucion)
                        {
                            if (contrPeriod.ComienzoPeriodo == eachRow.GetCell(0).DateCellValue)
                            {
                                periodExist = true;
                                rowResult += verifyRow(contrPeriod, eachRow, row + 1);
                                break;
                            } 
                        }
                        if (!periodExist)
                        {
                            rowResult += StartPeriodNotFound(row + 1);
                            verify = false;
                        }
                    }

                    if (rowResult != string.Empty)
                    {
                        verify = false;
                    }

                    if (verify)
                    {
                        result += "<p>La Cta. Seguridad Social " + sheetName + " ha sido verificada satisfactoriamente<p>";
                    } else
                    {
                        result += "<p>Error en la verificación para " + sheetName + ":</p>" + rowResult;
                    }
                        
                }
                
            }
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            
            return result;
        }



        private string StartPeriodNotFound(int row)
        {
            return @"<p style=""padding-left:5em"">" + "line: " + row.ToString() + " ningún registro para esa fecha de inicio</p>";
        }

        private string verifyRow(PeriodoContribucion contrPeriod, IRow eachRow, int i)
        {
            string result = "";
            if (contrPeriod.FinPeriodo == eachRow.GetCell(1).DateCellValue)
            {

                double money;
                if (eachRow.GetCell(2).CellType == CellType.Numeric)
                {
                    money = eachRow.GetCell(2).NumericCellValue;
                } else
                {
                    money = double.Parse(eachRow.GetCell(2).StringCellValue.Replace('.', ','));
                }

                if (contrPeriod.Dinero == money)
                {
                    result = string.Empty;
                } else
                {
                    result = @"<p style=""padding-left:5em"">" + "line: " + i.ToString() + " ningún registro con ese dinero contribuido</p>";
                }

            } else
            {
                result = @"<p style=""padding-left:5em"">" + "line: " + i.ToString() + " ningún registro para esa fecha final</p>";
            }
            return result;
        }
    }
}