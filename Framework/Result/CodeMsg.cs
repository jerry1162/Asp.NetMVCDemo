using System;
using System.Web.Mvc;

namespace Framework.Result
{
	public class CodeMsg
	{
		private int code { get; set; }

		private string msg { get; set; }

		private CodeMsg(int code, string msg)
		{
			this.code = code;
			this.msg = msg;
		}
		
		public CodeMsg FillArgs(params object[] msg)
		{
			this.msg = string.Format(this.msg, msg);
			return this;
		}
		
		public CodeMsg Code(int code)
		{
			this.code = code;
			return this;
		}

		public int Code()
		{
			return code;
		}

		public CodeMsg Msg(string msg)
		{
			this.msg = msg;
			return this;
		}

		public string Msg()
		{
			return msg;
		}
		
		/*下方开始结果的定义*/

		public static CodeMsg Success()
		{
			return new CodeMsg(0, "SUCCESS");
		}
		
		public static CodeMsg UnknownError()
		{
			return new CodeMsg(-1, "未知错误");
		}

		public static CodeMsg InvalidArg()
		{
			return new CodeMsg(100, "参数错误：{0}");
		}
	}
}