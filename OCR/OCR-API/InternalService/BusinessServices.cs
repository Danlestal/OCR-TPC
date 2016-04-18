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


        public User Authenticate(string userName, string password)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Name == userName && u.Password == password);
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