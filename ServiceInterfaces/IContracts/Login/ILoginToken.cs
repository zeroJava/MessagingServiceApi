namespace MessagingServiceInterfaces.IContracts.Login
{
	public interface ILoginToken
	{
		bool LoginSuccessful { get; set; }
		string UserName { get; set; }
		string UserEmailAddress { get; set; }
		//User User { get; set; }
	}
}