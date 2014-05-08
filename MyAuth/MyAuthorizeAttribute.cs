using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MyAuth.Service;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace MyAuth
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Calls when an action is being authorized.
        /// </summary>
        /// <param name="actionContext">The context.</param><exception cref="T:System.ArgumentNullException">The context parameter is null.</exception>
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var token = TokenService.GetTokenFromHeader();
            if (token != null && token.IsExpired())
                return;
            HandleUnauthorizedRequest(actionContext);        
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response = response;
        }
    }
}