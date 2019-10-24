using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Filters;
using Framework.Filters;
using Framework.Filters.WebApi;

namespace ApiFramework
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API 配置和服务

			// Web API 路由
			config.MapHttpAttributeRoutes();
			
			config.Filters.Add(new GlobalExceptionAttribute());
			//config.Filters.Add(new AuthFilterAttribute());
			config.Filters.Add(new ArgFilterAttribute());

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new {controller = "Values", action = "Index", id = RouteParameter.Optional}
			);
		}
	}
}