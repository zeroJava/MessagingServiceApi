namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
    public interface IRetrieveMessageRequest
    {
        string UserCredentials { get; set; }
        string Username { get; set; }

        string SenderEmailAddress { get; set; }
        string ReceiverEmailAddress { get; set; }

        long MessageIdThreshold { get; set; }
        int NumberOfMessages { get; set; }
    }
}