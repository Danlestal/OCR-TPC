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

            string result = "";

            FileStream _fileStream = new FileStream(filePath, FileMode.Open,
                                      FileAccess.Read);

            IWorkbook _workbook = WorkbookFactory.Create(_fileStream);
            _fileStream.Close();

            int sheetsNumber = _workbook.NumberOfSheets;

            for (int i = 0; i < sheetsNumber; i++)
            {
                ISheet _worksheet = _workbook.GetSheetAt(i);

                for (int row = 2; row <= _worksheet.LastRowNum; row++)
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

                    result += VerifyRow(eachRow, row);
                }

                result += @"<p style=""text-info"">" + "Nueva Pestaña</p>";
            }

            if (File.Exists(filePath))
                File.Delete(filePath); 

            return result;

        }

        private string VerifyRow(IRow eachRow, int row)
        {
            Contribuidor contributor = new Contribuidor();

            double idContributor = eachRow.GetCell(1).NumericCellValue;

            contributor = dbContext.Contributors.FirstOrDefault(s => s.IdentificadorSeguridadSocial == idContributor);
            if (contributor == null)
                return ContributorNotFound(row);

            //if (contributor != eachRow.GetCell(2).StringCellValue)
            //    return NIFNotFound(row);

            if (contributor.RazonSocial != eachRow.GetCell(3).StringCellValue)
                return RazonSocialNotFound(row);

            return Correct(row);
        }

        private string ContributorNotFound(int row)
        {
            return @"<p style=""text-danger"">" + "line: " + row.ToString() + " ningún registro para Cta. de vCotización</p>";
        }

        private string NIFNotFound(int row)
        {
            return @"<p style=""text-danger"">" + "line: " + row.ToString() + " ningún registro para NIF</p>";
        }

        private string RazonSocialNotFound(int row)
        {
            return @"<p style=""text-danger"">" + "line: " + row.ToString() + " ningún registro para Razón Social</p>";
        }

        private string Correct(int row)
        {
            return @"<p style=""text-success"">" + "line: " + row.ToString() + " Verificación Correcta</p>";
        }

    }