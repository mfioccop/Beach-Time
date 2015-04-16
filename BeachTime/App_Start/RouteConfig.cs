using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BeachTime
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			
			routes.MapRoute(
				name: "Error",
				url: "Error/{action}/{id}",
				defaults: new { controller = "Error", action = "Unknown", id = UrlParameter.Optional }
			);		

			routes.MapRoute(
				name: "Consultant Dashboard",
				url: "Consultant/{action}/{id}",
				defaults: new { controller = "Consultant", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Executive Dashboard",
				url: "Executive/{action}/{id}",
				defaults: new { controller = "Executive", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Admin Dashboard",
				url: "Admin/{action}/{id}",
				defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Account pages",
				url: "Account/{action}/{id}",
				defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Home",
				url: "{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);



			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
