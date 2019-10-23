using System.ComponentModel.DataAnnotations;

namespace ApiFramework.Models
{
	public class TestModel
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
	}
}