using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppTest.Models
{
	public class TestModel
	{
		[Required]
		public string Value { get; set; }
	}
}