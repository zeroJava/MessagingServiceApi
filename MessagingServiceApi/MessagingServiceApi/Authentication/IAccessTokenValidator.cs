namespace MessagingServiceApi.Authentication
{
	public interface IAccessTokenValidator
	{
		TokenValidationResult Validate(string encryptedToken);
		TokenValidationResult IsUserValid(string encryptedUserCred);
	}
}