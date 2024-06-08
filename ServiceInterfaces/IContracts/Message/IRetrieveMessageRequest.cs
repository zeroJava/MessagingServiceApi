namespace MessagingServiceInterfaces.IContracts.Message
{
	public interface IRetrieveMessageRequest
	{
		string UserAccessToken { get; set; }
		string Username { get; set; }
		string SenderEmailAddress { get; set; }
		string ReceiverEmailAddress { get; set; }
		long MessageIdThreshold { get; set; }
		int NumberOfMessages { get; set; }
	}
}