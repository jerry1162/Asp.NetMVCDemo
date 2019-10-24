using Framework.Authorization;

namespace Framework.Controllers
{
	public interface IController
	{
		IUser CurUser { get; set; }
	}
}