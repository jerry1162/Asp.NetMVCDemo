using System.Web;
using System.Web.Mvc;
using Framework.Filters.Mvc;

namespace ApiFramework
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			//filters.Add(new HandleErrorAttribute());
			filters.Add(new GlobalExceptionAttribute());
			filters.Add(new ArgFilterAttribute());
		}
	}
}
