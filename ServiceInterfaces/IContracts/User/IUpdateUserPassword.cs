namespace MessagingServiceInterfaces.IContracts.User
{
	public interface IUpdateUserPassword
	{
		string UserName { get; set; }
		string OldPassword { get; set; }
		string NewPassword { get; set; }
	}
}