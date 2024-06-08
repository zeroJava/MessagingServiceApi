namespace MessagingServiceInterfaces.IContracts.User
{
	public interface INewUserData : IUser
	{
		string Password { get; set; }
	}
}