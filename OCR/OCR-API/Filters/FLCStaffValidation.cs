using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OCR_API.Filters
{
    public class FLCStaffValidation
    {
        public static bool ValidateUserFLC(string user, string pwd, ref string msg)
        {
            try
            {
               
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["desarrollo"]))
                {
                   return true;
                }

                msg = ValidateUserSP(user, pwd); 
                if (msg == "Usuario conectado correctamente")
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ValidateUserFLC(string app, string user, string pwd, ref string msg)
        {
            try
            {
                bool UserOk = false;
                msg = ValidateUserSP(app, user, pwd);

                if ((msg == "Usuario conectado correctamente") || (Convert.ToBoolean(ConfigurationManager.AppSettings["desarrollo"])))
                {
                    UserOk = true;
                }

                return UserOk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string ValidateUserSP(string user, string pwd)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["PortalEmpleadoConnectionString"].ToString();
                var app = ConfigurationManager.AppSettings["app"].ToString();
                using (var con = new SqlConnection(connectionString))
                {
                    var cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "login_usuarios_flc",
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@aplicacion", app);
                    cmd.Parameters.AddWithValue("@usuario", user);
                    cmd.Parameters.AddWithValue("@password", pwd);

                    AddParams(ref cmd);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return cmd.Parameters["@mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string ValidateUserSP(string app, string user, string pwd)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["PortalEmpleadoConnectionString"].ToString();
                using (var con = new SqlConnection(connectionString))
                {
                    var cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "login_usuarios_flc",
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@aplicacion", app);
                    cmd.Parameters.AddWithValue("@usuario", user);
                    cmd.Parameters.AddWithValue("@password", pwd);

                    AddParams(ref cmd);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return cmd.Parameters["@mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void AddParams(ref SqlCommand cmd)
        {
            try
            {
                var error_login = new SqlParameter
                {
                    ParameterName = "@error_login",
                    Value = string.Empty,
                    Direction = ParameterDirection.Output,
                    DbType = DbType.Int32
                };
                cmd.Parameters.Add(error_login);

                var perfil1 = new SqlParameter
                {
                    ParameterName = "@perfil1",
                    Value = string.Empty,
                    Direction = ParameterDirection.Output,
                    Size = 100,
                    DbType = DbType.AnsiString
                };
                cmd.Parameters.Add(perfil1);

                var perfil2 = new SqlParameter
                {
                    ParameterName = "@perfil2",
                    Value = string.Empty,
                    Direction = ParameterDirection.Output,
                    Size = 100,
                    DbType = DbType.AnsiString
                };
                cmd.Parameters.Add(perfil2);

                var perfil3 = new SqlParameter
                {
                    ParameterName = "@perfil3",
                    Value = string.Empty,
                    Direction = ParameterDirection.Output,
                    Size = 100,
                    DbType = DbType.AnsiString
                };
                cmd.Parameters.Add(perfil3);

                var perfil4 = new SqlParameter
                {
                    ParameterName = "@perfil4",
                    Value = string.Empty,
                    Direction = ParameterDirection.Output,
                    Size = 100,
                    DbType = DbType.AnsiString
                };
                cmd.Parameters.Add(perfil4);

                var perfil5 = new SqlParameter
                {
                    ParameterName = "@perfil5",
                    Value = string.Empty,
                    Direction = ParameterDirection.Output,
                    Size = 100,
                    DbType = DbType.AnsiString
                };
                cmd.Parameters.Add(perfil5);

                var pMensaje = new SqlParameter
                {
                    ParameterName = "@mensaje",
                    Value = string.Empty,
                    Direction = ParameterDirection.Output,
                    Size = 1000,
                    DbType = DbType.AnsiString
                };
                cmd.Parameters.Add(pMensaje);

                var pGrupo = new SqlParameter
                {
                    ParameterName = "@grupo",
                    Value = string.Empty,
                    Direction = ParameterDirection.Output,
                    Size = 25,
                    DbType = DbType.AnsiString
                };
                cmd.Parameters.Add(pGrupo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}