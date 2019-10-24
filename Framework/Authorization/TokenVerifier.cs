using System;

namespace Framework.Authorization
{
	public class TokenVerifier: ITokenVerifier
	{
		public IUser Verify(string token)
		{
			Console.WriteLine($"token:{token} ok");
			return null;
		}
	}
}