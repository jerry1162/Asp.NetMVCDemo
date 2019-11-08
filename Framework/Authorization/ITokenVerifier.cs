namespace Framework.Authorization
{
	/// <summary>
	/// Token验证器，验证成功返回用户信息，失败返回null
	/// </summary>
	public interface ITokenVerifier
	{
		UserBase Verify(string token);
	}
}