using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiFramework.Models;
using Framework.Authorization;
using Framework.Filters;
using Framework.Filters.WebApi;
using Framework.Validator;

namespace ApiFramework.Controllers
{
	[AuthFilter(Verifier = typeof(TokenVerifier))]
	public class ValuesController : ApiController
	{
		// GET api/values
		//public IEnumerable<string> Get()
		//{
		//	return new string[] {"value1", "value2"};
		//}

		// GET api/values/5
		public string Get([ValidArg] TestModel model)
		{
			Console.WriteLine(model);
			return model?.Name ?? "";
		}

		// POST api/values
		public void Post([FromBody][ValidArg] TestModel model)
		{
			Console.WriteLine(model.ToString());
		}
	}
}