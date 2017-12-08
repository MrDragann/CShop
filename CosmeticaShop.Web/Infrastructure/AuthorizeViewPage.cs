using System;
using System.Web.Mvc;

namespace CosmeticaShop.Web.Infrastructure
{
    public class AuthorizeViewPage : WebViewPage
    {
        public AuthorizeViewPage()
        {
            User = new WebUser();
        }

        public new WebUser User { get; set; }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }

    public class AuthorizeViewPage<TModel> : WebViewPage<TModel>
    {
        public AuthorizeViewPage()
        {
            User = new WebUser();
        }

        public new WebUser User { get; set; }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}