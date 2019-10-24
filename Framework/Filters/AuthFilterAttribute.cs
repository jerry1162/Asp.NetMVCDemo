using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Framework.Authorization;
using Framework.Common;
using Framework.Exception;
using Framework.Result;
using HttpMethod = Framework.Common.Constants.HttpMethod;

namespace Framework.Filters
{
	public class AuthFilterAttribute : AuthorizationFilterAttribute
	{
		public Type Verifier { get; set; }
		
		private ITokenVerifier TokenVerifier { get; set; }

		public override void OnAuthorization(HttpActionContext actionContext)
		{
			base.OnAuthorization(actionContext);
			var actionDescriptor = actionContext.ActionDescriptor;
			var controllerDescriptor = actionDescriptor.ControllerDescriptor;

			//是否允许匿名访问
			if (controllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ||
			    actionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
			{
				return;
			}
			
			var token = GetToken(actionContext);
			
			//识别器不存在则读取识别器
			if (TokenVerifier == null) ;
			{
				if (!typeof(ITokenVerifier).IsAssignableFrom(Verifier))
				{
					throw CodeMsg.InvalidVerifier().BuildError();
				}

				var verifier = Verifier.GetConstructor(Type.EmptyTypes)?.Invoke(null);
				if (!(verifier is ITokenVerifier tokenVerifier))
					throw CodeMsg.InvalidVerifier().BuildError();
				
				TokenVerifier = verifier as ITokenVerifier;
			}
			if (TokenVerifier.Verify(token) == null)
			{
				throw CodeMsg.InvalidToken().BuildError();
			}
		}

		/// <summary>
		/// 从请求中获取token，失败则抛出异常
		/// </summary>
		/// <param name="actionContext"></param>
		/// <returns></returns>
		/// <exception cref="Error"></exception>
		private static string GetToken(HttpActionContext actionContext)
		{
			var token = TryGetToken(actionContext);
			if (token == null)
			{
				throw CodeMsg.TokenRequired().BuildError();
			}

			return token;
		}

		/// <summary>
		/// 尝试从本次请求中获取Token，可能返回null
		/// </summary>
		/// <param name="actionContext"></param>
		/// <returns></returns>
		private static string TryGetToken(HttpActionContext actionContext)
		{
			//TODO 暂未做从Cookie中获取
			//先从Authorization字段中读取Token，读不到再从请求参数中读取
			return actionContext.Request.Headers.Authorization?.Parameter ?? GetTokenForGetOrPost(actionContext);
		}

		private static string GetTokenForGetOrPost(HttpActionContext actionContext)
		{
			if (HttpMethod.GET.Equals(actionContext.Request.Method.Method))
			{
				var queryString = actionContext.Request.RequestUri.ParseQueryString();
				return queryString[Constants.Token.NAME];
			}
			else
			{
				return HttpContext.Current.Request.Form[Constants.Token.NAME];
			}
		}
	}
}