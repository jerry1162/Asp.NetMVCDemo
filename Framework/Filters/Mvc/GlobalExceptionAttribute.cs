using System.Web.Mvc;
using Framework.Common;
using Framework.Exception;
using Framework.Result;

namespace Framework.Filters.Mvc
{
	public class GlobalExceptionAttribute : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			base.OnException(filterContext);
			filterContext.ExceptionHandled = true;
			
			CodeMsg codeMsg;
			var exception = filterContext.Exception;
			if (exception is Error error)
			{
				codeMsg = error.CodeMsg;
			}
			else
			{
				codeMsg = CodeMsg.UnknownError().Msg(exception.Message);
			}

			var method = filterContext.HttpContext.Request.HttpMethod;
			if (Constants.HttpMethod.GET.Equals(method))
			{
				filterContext.Result = codeMsg.BuildContentResult();
			}
			else
			{
				filterContext.Result = codeMsg.BuildJsonResult();
			}
		}
	}
}