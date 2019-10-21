using System.Web;
using System.Web.Mvc;
using AppTest.Filters;

namespace AppTest
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			filters.Add(new ArgFilterAttribute());
		}
	}
}
