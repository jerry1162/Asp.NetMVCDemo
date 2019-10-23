using System.Web.Mvc;
using ApiFramework.Models;
using Framework.Validator;

namespace ApiFramework.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Test([ValidArg] TestModel model)
		{
			return new ContentResult();
		}
	}
}