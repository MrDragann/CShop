using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmeticaShop.Web.Infrastructure
{
    public class AuthorizationAttributePublic : AuthorizeAttribute
    {



        private WebUser _user => new WebUser();


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!_user.IsAuthorized)
                return false;

            return true;
        }

        /// <summary>
        /// Редирект в случае если пользователю отказано в доступе
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var returnUrl = filterContext.HttpContext.Request.Url;
            filterContext.Result = string.IsNullOrWhiteSpace(returnUrl?.PathAndQuery) ? new RedirectResult("/Home/Index", false)
                : new RedirectResult($"/Home/Index?returnUrl={returnUrl.PathAndQuery}", false);
        }

    }
}