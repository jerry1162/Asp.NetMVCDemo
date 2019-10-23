using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Framework.Exception;
using Framework.Result;
using Framework.Validator;

namespace Framework.Filters.WebApi
{
	public class ArgFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var stringBuilder = new StringBuilder();
			var modelState = actionContext.ModelState;
			if (!modelState.IsValid)
			{
				foreach (var item in modelState.Values)
				{
					foreach (var error in item.Errors)
					{
						stringBuilder.AppendLine(error.ErrorMessage);
					}
				}
				throw CodeMsg.InvalidArg().FillArgs(stringBuilder.ToString()).BuildError();
			}

			var actionDescriptor = actionContext.ActionDescriptor;
			var controllerDescriptor = actionDescriptor.ControllerDescriptor;
			var args = actionContext.ActionArguments;
			var argDefInfo = actionDescriptor.GetParameters();
			foreach (var argDef in argDefInfo)
			{
				var validArg = argDef.GetCustomAttributes<ValidArgAttribute>().FirstOrDefault();
				if (validArg != null)
				{
					if (validArg.Required && args[argDef.ParameterName] == null)
					{
						stringBuilder.AppendLine(validArg.Msg);
					}
				}
				else if (argDef.ParameterType.IsPrimitive && args[argDef.ParameterName] == null)
				{
					stringBuilder.AppendLine($"{argDef.ParameterName}不能为空");
				}
			}

			if (stringBuilder.Length > 0)
			{
				throw CodeMsg.InvalidArg().FillArgs(stringBuilder.ToString()).BuildError();
			}
			base.OnActionExecuting(actionContext);
		}
	}
}