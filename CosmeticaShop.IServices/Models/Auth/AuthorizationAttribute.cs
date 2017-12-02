using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Models.User;

namespace CosmeticaShop.IServices.Models.Auth
{
    /// <summary>
    /// Аттрибут авторизации
    /// </summary>
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        public AuthorizationAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }

        private WebUser user => new WebUser();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!user.IsAuthorized)
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
