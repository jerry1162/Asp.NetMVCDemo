using Framework.Authorization;

namespace Framework.Controllers
{
	public interface IController
	{
		UserBase CurUser { get; set; }
	}
}