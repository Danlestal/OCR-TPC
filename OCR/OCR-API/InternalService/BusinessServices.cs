using OCR_API.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCR_API.InternalService
{
    public class BusinessServices
    {

        private OCR_TPC_Context dbContext;

        public BusinessServices(OCR_TPC_Context context)
        {
            dbContext = context;

        }

        public string LogIn(string userName, string password)
        {
            
            var user = dbContext.Users.FirstOrDefault(u => u.Nombre == userName);
            if (user == null)
            {
                return "No se encontró el usuario";
            }
            else
            {
                user = dbContext.Users.FirstOrDefault(u => u.Nombre == userName && u.Password == password);
                if (user == null)
                {
                    return "Contraseña incorrecta";
                }
            }
            return string.Empty;
        }

        public Usuario Authenticate(string userName, string password)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Nombre == userName && u.Password == password);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new ArgumentException("No user found");
            }
            
        }
    }
}