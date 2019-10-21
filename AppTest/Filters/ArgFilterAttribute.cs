using System;
using System.Text;
using System.Web.Mvc;
using AppTest.Common.Constants;
using AppTest.Result;
using AppTest.Utils;

namespace AppTest.Filters
{
	public class ArgFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var stringBuilder = new StringBuilder();
			var modelState = filterContext.Controller.ViewData.ModelState;
			if (!modelState.IsValid)
			{
				foreach (var item in modelState.Values)
				{
					foreach (var error in item.Errors)
					{
						stringBuilder.AppendLine(error.ErrorMessage);
					}
				}
				
				HandleResult(filterContext, CodeMsg.ArgValidateError().FillArgs(stringBuilder.ToString()));
				return;
			}

			var actionDescriptor = filterContext.ActionDescriptor;
			var controllerDescriptor = actionDescriptor.ControllerDescriptor;
			var args = filterContext.ActionParameters;
			var argDefInfo = actionDescriptor.GetParameters();
			foreach (var argDef in argDefInfo)
			{
				if (argDef.ParameterType.IsPrimitive && args[argDef.ParameterName] == null)
				{
					stringBuilder.AppendLine($"{argDef.ParameterName}不能为空");
				}
			}

			if (stringBuilder.Length > 0)
			{
				HandleResult(filterContext, CodeMsg.ArgValidateError().FillArgs(stringBuilder.ToString()));
				return;
			}
			
			//Console.WriteLine(controllerDescriptor.ControllerName);
			//Console.WriteLine(actionDescriptor.ActionName);
			//Console.WriteLine(args.ToString());

			base.OnActionExecuting(filterContext);
		}

		private static void HandleResult(ActionExecutingContext filterContext, CodeMsg codeMsg)
		{
			var method = filterContext.HttpContext.Request.HttpMethod;
			switch (method)
			{
				case HttpMethod.GET:
					filterContext.Result = new ContentResult()
					{
						Content = codeMsg.Msg
					};
					break;
				default:
					filterContext.Result = codeMsg.BuildJsonResult();
					break;
			}
		}
	}
}