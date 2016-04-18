using OCR_API.DataLayer;
using OCR_API.InternalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;

namespace OCR_API.Filters
{
    public class ApiAuthenticationFilter : GenericAuthenticationFilter
    {

        private OCR_TPC_Context dbContext;


        /// <summary>
        /// Default Authentication Constructor
        /// </summary>
        public ApiAuthenticationFilter(OCR_TPC_Context context)
        {
            dbContext = context;
        }

        /// <summary>
        /// AuthenticationFilter constructor with isActive parameter
        /// </summary>
        /// <param name="isActive"></param>
        public ApiAuthenticationFilter(bool isActive)
            : base(isActive)
        {
        }

        /// <summary>
        /// Protected overriden method for authorizing user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            var businessService = new BusinessServices(dbContext);

                var user = businessService.Authenticate(username, password);
                if ( user != null )
                {
                    var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                    if (basicAuthenticationIdentity != null)
                    {
                        basicAuthenticationIdentity.UserName = user.Name;
                        basicAuthenticationIdentity.Password = user.Password;
                        return true;
                    }
                return false;
                }
            return false;
        }
    }
}
