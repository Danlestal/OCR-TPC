using OCR_API.DataLayer;
using OCR_API.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            string response = string.Empty;
            if (FLCStaffValidation.ValidateUserFLC(userName, password, ref response))
            {
                var plainTextBytes = Encoding.Default.GetBytes(userName+":"+password);
                return System.Convert.ToBase64String(plainTextBytes);
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