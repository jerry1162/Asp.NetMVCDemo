using System.Web.Mvc;
using AppTest.Utils;

namespace AppTest.Result
{
	public class Result
	{
		public int Code { get; set; }

		public string Msg { get; set; }

		public object Data { get; set; }

		private Result(int code, string msg, object data)
		{
			Code = code;
			Msg = msg;
			Data = data;
		}

		public static Result Build(CodeMsg codeMsg, object data = null)
		{
			return new Result(codeMsg.Code, codeMsg.Msg, data);
		}

		public static Result Success(object data, CodeMsg codeMsg = null)
		{
			if (codeMsg == null)
			{
				codeMsg = CodeMsg.Success();
			}

			return new Result(codeMsg.Code, codeMsg.Msg, data);
		}

		public static Result Error(CodeMsg codeMsg, object data = null)
		{
			return new Result(codeMsg.Code, codeMsg.Msg, data);
		}

		public JsonResult Json()
		{
			return JsonUtil.Json(this);
		}
	}
}