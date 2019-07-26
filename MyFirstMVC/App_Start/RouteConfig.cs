using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyFirstMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //https: //localhost: 44398/ anasayfa  => burda anasayfa yazmasını sağladık.
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("kvkkRoute", "kvkk", new { controller = "Home", action = "Kvkk" }, namespaces: new string[] { "MyFirstMVC.Controllers" });
            routes.MapRoute("anasayfaRoute", "anasayfa", new { controller = "Home", action = "Index" }, namespaces: new string[] { "MyFirstMVC.Controllers" });
            routes.MapRoute("projelerRoute", "projeler", new { controller = "Home", action = "Project" }, namespaces: new string[] { "MyFirstMVC.Controllers" });
            routes.MapRoute("iletisimRoute", "iletisim", new { controller = "Home", action = "Contact" }, namespaces: new string[] { "MyFirstMVC.Controllers" });
            routes.MapRoute("hakkımızdaRoute", "hakkımızda", new { controller = "Home", action = "About" }, namespaces: new string[] { "MyFirstMVC.Controllers" });
            
            //kullanım koşulları ve gizlilik politikası eklenecek kvkk nın oraya.

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }, namespaces: new string[] {"MyFirstMVC.Controllers"}

            );
        }
    }
}
