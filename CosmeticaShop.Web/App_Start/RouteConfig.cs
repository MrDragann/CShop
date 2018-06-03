using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CosmeticaShop.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ProductDetail",
                url: "produs/{productKeyUrl}",
                defaults: new { controller = "Product", action = "Detail", productKeyUrl = UrlParameter.Optional },
                namespaces: new[] { "CosmeticaShop.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Blog",
                url: "blog",
                defaults: new { controller = "Blog", action = "Index" },
                namespaces: new[] { "CosmeticaShop.Web.Controllers" }
            );

            routes.MapRoute(
                name: "BlogDetail",
                url: "blog/{keyUrl}",
                defaults: new { controller = "Blog", action = "Detail", keyUrl = UrlParameter.Optional },
                namespaces: new[] { "CosmeticaShop.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CosmeticaShop.Web.Controllers" }
            );
        }
    }
}
