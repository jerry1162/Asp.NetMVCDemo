using System;

namespace Framework.Validator
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class ValidArgAttribute : Attribute
	{
		public bool Required { get; set; } = true;

		public string Msg { get; set; } = "参数校验失败";
	}
}