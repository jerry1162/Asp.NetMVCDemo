using System;
using System.Web.Mvc;

namespace AppTest.Result
{
	public class CodeMsg
	{
		public int Code { get; set; }

		public string Msg { get; set; }

		private CodeMsg(int code, string msg)
		{
			Code = code;
			Msg = msg;
		}

		public static CodeMsg Success()
		{
			return new CodeMsg(0, "SUCCESS");
		}
		
		public static CodeMsg UnknownError()
		{
			return new CodeMsg(-1, "未知错误");
		}

		public static CodeMsg ArgValidateError()
		{
			return new CodeMsg(100, "参数错误：{0}");
		}

		public CodeMsg FillArgs(params object[] msg)
		{
			Msg = string.Format(Msg, msg);
			return this;
		}

		public Result Build()
		{
			return Result.Build(this);
		}

		public JsonResult BuildJsonResult()
		{
			return Build().Json();
		}
	}
}