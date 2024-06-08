namespace MessagingServiceInterfaces.IContracts.Errors
{
	public interface IError
	{
		string Message { get; set; }
		int Status { get; set; }
	}
}