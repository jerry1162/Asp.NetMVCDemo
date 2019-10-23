using System.Text;
using System.Web.Mvc;
using Framework.Exception;
using Framework.Utils;

namespace Framework.Result
{
	public static class Builder
	{
		public static Error BuildError(this CodeMsg codeMsg)
		{
			return Error.Build(codeMsg);
		}
		
		public static Result Build(this CodeMsg codeMsg)
		{
			return Result.Build(codeMsg);
		}
		public static JsonResult BuildJsonResult(this CodeMsg codeMsg)
		{
			return JsonUtil.Json(codeMsg.Build());
		}
		
		public static ContentResult BuildContentResult(this CodeMsg codeMsg)
		{
			return new ContentResult()
			{
				Content = codeMsg.Msg(),
				ContentEncoding = Encoding.Default
			};
		}
	}
}