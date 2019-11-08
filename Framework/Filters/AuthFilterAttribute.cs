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
		public Type VerifierType
		{
			get;
			set;
			/*
			{

			}
			*/
		}

		private ITokenVerifier _tokenVerifier;

		private ITokenVerifier TokenVerifier
		{
			get
			{
				if (_tokenVerifier != null) return _tokenVerifier;
				//验证器不存在则读取验证器
				if (!typeof(ITokenVerifier).IsAssignableFrom(VerifierType))
				{
					throw CodeMsg.InvalidVerifier().BuildError();
				}

				var verifier = VerifierType.GetConstructor(Type.EmptyTypes)?.Invoke(null);
				if (!(verifier is ITokenVerifier tokenVerifier))
					throw CodeMsg.InvalidVerifier().BuildError();

				_tokenVerifier = verifier as ITokenVerifier;

				return _tokenVerifier;
			}
		}

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
			if (string.IsNullOrEmpty(token))
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
			//先从Cookie中读取Token，然后从Authorization字段中读取Token，读不到再从请求参数中读取
			var token = GetTokenFromCookie(actionContext);
			if (!string.IsNullOrEmpty(token))
			{
				return token;
			}
			return actionContext.Request.Headers.Authorization?.Parameter ?? GetTokenForGetOrPost(actionContext);
		}

		private static string GetTokenFromCookie(HttpActionContext actionContext)
		{
			var collection = actionContext.Request.Headers.GetCookies(Constants.Keys.TOKEN);
			var value = "";
			if (collection.Count > 0)
			{
				value = collection[0][Constants.Keys.TOKEN].Value;
			}

			return value;
		}

		/// <summary>
		/// 从请求参数中获取Token信息
		/// </summary>
		/// <param name="actionContext"></param>
		/// <returns></returns>
		private static string GetTokenForGetOrPost(HttpActionContext actionContext)
		{
			if (HttpMethod.GET.Equals(actionContext.Request.Method.Method))
			{
				var queryString = actionContext.Request.RequestUri.ParseQueryString();
				return queryString[Constants.Keys.TOKEN];
			}
			else
			{
				return HttpContext.Current.Request.Form[Constants.Keys.TOKEN];
			}
		}
	}
}