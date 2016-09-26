using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace OCR_API.Filters
{
    public class ApiAuthenticationFilter : AuthorizationFilterAttribute
    {
        private FLCStaffValidation validator;
        private bool isActive;


        /// <summary>
        /// AuthenticationFilter constructor with isActive parameter
        /// </summary>
        /// <param name="isActive"></param>
        public ApiAuthenticationFilter(bool isActive)
        {
            validator = new FLCStaffValidation();
            this.isActive = isActive;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!isActive) return;
            var identity = FetchAuthHeader(actionContext);

            if (identity == null)
            {
                CreateUnauthorizedResponse(actionContext);
                return;
            }

            var genericPrincipal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = genericPrincipal;

            if (!CheckIdentity(identity.Name, identity.Password, actionContext))
            {
                CreateUnauthorizedResponse(actionContext);
                return;
            }

            base.OnAuthorization(actionContext);
        }

        protected bool CheckIdentity(string username, string password, HttpActionContext actionContext)
        {
            string response = string.Empty;
            return FLCStaffValidation.ValidateUserFLC(username, password, ref response);
        }


        private static void CreateUnauthorizedResponse(HttpActionContext filterContext)
        {
            var dnsHost = filterContext.Request.RequestUri.DnsSafeHost;
            filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            filterContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", dnsHost));
        }

        protected virtual BasicAuthenticationIdentity FetchAuthHeader(HttpActionContext filterContext)
        {
            string authHeaderValue = null;
            var authRequest = filterContext.Request.Headers.Authorization;

            if (authRequest != null && !String.IsNullOrEmpty(authRequest.Scheme))
                authHeaderValue = authRequest.Parameter;

            if (string.IsNullOrEmpty(authHeaderValue))
                return null;

            authHeaderValue = Encoding.Default.GetString(Convert.FromBase64String(authHeaderValue));
            var credentials = authHeaderValue.Split(':');
            return credentials.Length < 2 ? null : new BasicAuthenticationIdentity(credentials[0], credentials[1]);
        }
    }
}
