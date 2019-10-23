using Framework.Result;

namespace Framework.Exception
{
	public class Error: System.Exception
	{
		public CodeMsg CodeMsg { get; set; }

		private Error(CodeMsg codeMsg)
		{
			CodeMsg = codeMsg;
		}

		public static Error Build(CodeMsg codeMsg)
		{
			return new Error(codeMsg);
		}
	}
}