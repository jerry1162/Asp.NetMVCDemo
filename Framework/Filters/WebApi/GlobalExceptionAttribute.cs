using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Framework.Exception;
using Framework.Result;
using HttpMethod = Framework.Common.Constants.HttpMethod;

namespace Framework.Filters.WebApi
{
	public class GlobalExceptionAttribute : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			CodeMsg codeMsg;
			var exception = actionExecutedContext.Exception;
			if (exception is Error error)
			{
				codeMsg = error.CodeMsg;
			}
			else
			{
				codeMsg = CodeMsg.UnknownError().Msg(exception.Message);
			}
			var method = actionExecutedContext.Request.Method.Method;
			object value;
			if (HttpMethod.GET.Equals(method))
			{
				value = codeMsg.Msg();
			}
			else
			{
				value = codeMsg.Build();
			}

			actionExecutedContext.Response =
				actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest, value);
		}
	}
}