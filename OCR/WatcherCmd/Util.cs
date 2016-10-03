using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace WatcherCmd
{
    public class Util
    {
        public static int DeleteFilesToDelete()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["OCR_TPC_ConnectionString"].ToString();
                int iRegistrosEliminados = 0;
                using (var con = new SqlConnection(connectionString))
                {
                    var cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SELECT PathAbsolutoArchivo FROM PdfToDeletes",
                        CommandType = CommandType.Text
                    };

                    con.Open();
                    var lector = cmd.ExecuteReader();
                    while (lector.Read())
                    {
                        if (File.Exists(lector[0].ToString()))
                        {
                            File.Delete(lector[0].ToString());
                        }
                    }
                    con.Close();

                    var cmdDel = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "DELETE PdfToDeletes",
                        CommandType = CommandType.Text
                    };
                    con.Open();
                    iRegistrosEliminados = cmdDel.ExecuteNonQuery();
                    con.Close();

                    return iRegistrosEliminados;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
