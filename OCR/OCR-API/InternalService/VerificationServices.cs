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
        private int numRegistrosTotales;
        private int numRegistrosCorrectos;

        public VerificationServices(OCR_TPC_Context context)
        {
            dbContext = context;
            numRegistrosTotales = 0;
            numRegistrosCorrectos = 0;
        }

        public string FileVerification(string filePath)
        {
            numRegistrosTotales = 0;
            numRegistrosCorrectos = 0;
            string result = "";

            FileStream _fileStream = new FileStream(filePath, FileMode.Open,
                                      FileAccess.Read);

            IWorkbook _workbook = WorkbookFactory.Create(_fileStream);
            _fileStream.Close();

            int sheetsNumber = _workbook.NumberOfSheets;

            for (int i = 0; i < sheetsNumber; i++)
            {
                ISheet _worksheet = _workbook.GetSheetAt(i);

                if (i > 0)
                {
                    result += @"<p style=""text-info"">" + "Nueva Pestaña</p>";
                }

                for (int row = 2; row <= _worksheet.LastRowNum; row++)
                {

                    IRow eachRow = _worksheet.GetRow(row);
                    if (eachRow == null) //null is when the row only contains empty cells 
                    {
                        continue;
                    }
                    if (eachRow.GetCell(0) == null)
                    {
                        continue;
                    }

                    result += VerifyRow(eachRow, row);
                }

            }

            if (File.Exists(filePath))
                File.Delete(filePath);

            if (string.IsNullOrEmpty(result))
            {
                return @" < p style = ""color: red"" > Algo raro hay en el formato</p>";
            }
            else
            {
                result += @" <p style=""color: green""> Num registros correctos: " + numRegistrosCorrectos + "</p>";
                result += @" <p> Num registros totales: " + numRegistrosTotales + "</p>";
                return result;
            }

        }

        private string ReadCellAsAString(ICell cellToRead)
        {
            string result = string.Empty;
            if (cellToRead.CellType == NPOI.SS.UserModel.CellType.Numeric)
            {
                return cellToRead.NumericCellValue.ToString();
            }

            if (cellToRead.CellType == NPOI.SS.UserModel.CellType.String)
            {
                return cellToRead.StringCellValue;
            }

            throw new Exception();
        }

        private string VerifyRow(IRow eachRow, int row)
        {
            numRegistrosTotales++;
            Contribuidor contributor = new Contribuidor();

            ICell cell = eachRow.GetCell(1);

            string idContributor = ReadCellAsAString(eachRow.GetCell(1));
            contributor = dbContext.Contributors.FirstOrDefault(s => s.CuentaCotizacion == idContributor);
            if (contributor == null)
            {
                return ContributorNotFound(row, idContributor);
            }
        


            string contributorNIF = ReadCellAsAString(eachRow.GetCell(2));
            if (contributor.NIF != contributorNIF)
                return NIFNotFound(row, contributorNIF.ToString());

           string contributorRazonSocial = ReadCellAsAString(eachRow.GetCell(3));
           if (contributor.RazonSocial != contributorRazonSocial)
                return RazonSocialNotFound(row, contributorRazonSocial);

            numRegistrosCorrectos++;
            return Correct(row, idContributor);
        }

        private string ContributorNotFound(int row, string contributor)
        {
            return @"<p style=""color:red"">" + "fila: " + row.ToString() + " ningún registro para Cta. de Cotización: " + contributor + "</p>";
        }

        private string NIFNotFound(int row, string nif)
        {
            return @"<p style=""color:red"">" + "fila: " + row.ToString() + " ningún registro para NIF: " + nif + "</p>";
        }

        private string RazonSocialNotFound(int row, string razonSocial)
        {
            return @"<p style=""color:red"">" + "fila: " + row.ToString() + " ningún registro para Razón Social" + razonSocial + "</p>";
        }

        private string Correct(int row, string contribuidor)
        {
            return @"<p style=""color:green"">" + "fila: " + row.ToString() + " Verificación Correcta para Cta. de Cotización: " + contribuidor + "</p>";
        }

    }
}