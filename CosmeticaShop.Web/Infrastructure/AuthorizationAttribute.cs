using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmeticaShop.Web.Infrastructure
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

        private WebUser _user => new WebUser();

        /// <summary>
        /// Допустимые роли
        /// </summary>
        private List<string> _allowedRoles;
        /// <summary>
        /// Роли пользователя
        /// </summary>
        private List<string> _userRoles;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!_user.IsAuthorized)
                return false;

            _userRoles = _user.Roles;

            if (_userRoles==null)
                return false;

            _allowedRoles = Roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            
            //проверка пользователя на существование роли
            if (_allowedRoles.Any() && !_allowedRoles.Any(x => _userRoles.Contains(x)))
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
            filterContext.Result = string.IsNullOrWhiteSpace(returnUrl?.PathAndQuery) ? new RedirectResult("/Admin/Home/Index", false)
                : new RedirectResult($"/Admin/Home/Index?returnUrl={returnUrl.PathAndQuery}", false);
        }

    }
}
