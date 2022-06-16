namespace WMessageServiceApi.Messaging.DataContracts.UserContracts
{
	public interface INewUserDataContract : IUserContract
	{
		string Password { get; set; }
	}
}
