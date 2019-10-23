using System.Text;
using System.Web.Mvc;

namespace Framework.Utils
{
	public static class JsonUtil
	{
		public static JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			return new JsonResult()
			{
				Data = data,
				ContentType = contentType,
				ContentEncoding = contentEncoding,
				JsonRequestBehavior = behavior
			};
		}
		
		public static JsonResult Json(object data)
		{
			return new JsonResult()
			{
				Data = data,
				ContentType = null,
				ContentEncoding = null,
				JsonRequestBehavior = JsonRequestBehavior.DenyGet
			};
		}

		public static JsonResult Json(object data, JsonRequestBehavior behavior)
		{
			return new JsonResult()
			{
				Data = data,
				ContentType = null,
				ContentEncoding = null,
				JsonRequestBehavior = behavior
			};
		}
	}
}