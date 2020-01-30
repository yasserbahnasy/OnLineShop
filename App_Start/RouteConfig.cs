using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnLineShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "product",
               url: "product/{id}",
               defaults: new { controller = "Home", action = "Product" }
           );



            routes.MapRoute(
               name: "Category",
               url: "Category/{name}",
               defaults: new { controller = "Home", action = "Category" }
           );

            routes.MapRoute(
               name: "brand",
               url: "brand/{name}",
               defaults: new { controller = "Home", action = "brand" }
           );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
