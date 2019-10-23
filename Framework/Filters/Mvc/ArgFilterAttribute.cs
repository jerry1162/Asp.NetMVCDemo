using System;
using System.Text;
using System.Web.Mvc;
using Framework.Exception;
using Framework.Result;
using Framework.Validator;

namespace Framework.Filters.Mvc
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
				
				throw CodeMsg.InvalidArg().FillArgs(stringBuilder.ToString()).BuildError();
			}

			var actionDescriptor = filterContext.ActionDescriptor;
			var controllerDescriptor = actionDescriptor.ControllerDescriptor;
			var args = filterContext.ActionParameters;
			var argDefInfo = actionDescriptor.GetParameters();
			foreach (var argDef in argDefInfo)
			{
				if (argDef.IsDefined(typeof(ValidArgAttribute),false))
				{
					var validArg = argDef.GetCustomAttributes(typeof(ValidArgAttribute), false)[0] as ValidArgAttribute;
					if (validArg?.Required == true && args[argDef.ParameterName] == null)
					{
						stringBuilder.AppendLine(validArg.Msg);
					}
				}
				if (argDef.ParameterType.IsPrimitive && args[argDef.ParameterName] == null)
				{
					stringBuilder.AppendLine($"{argDef.ParameterName}不能为空");
				}
			}

			if (stringBuilder.Length > 0)
			{
				throw CodeMsg.InvalidArg().FillArgs(stringBuilder.ToString()).BuildError();
			}
			
			base.OnActionExecuting(filterContext);
		}
	}
}