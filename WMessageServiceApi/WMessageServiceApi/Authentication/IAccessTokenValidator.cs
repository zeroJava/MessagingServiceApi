namespace WMessageServiceApi.Authentication
{
	public interface IAccessTokenValidator
	{
		TokenValidationResult IsTokenValid(string encryptedToken);
		TokenValidationResult IsUserCredentialValid(string encryptedUserCred);
	}
}