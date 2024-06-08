using MessagingServiceInterfaces.Contracts.Message;
using MessagingServiceInterfaces.IContracts.Message;
using System.Collections.Generic;
using System.ServiceModel;

namespace MessagingServiceInterfaces.Services
{
	[ServiceContract]
	[ServiceKnownType(typeof(RetrieveMessageRequest))]
	public interface IGetMessageService
	{
		[OperationContract]
		List<PostedMessageInfo> GetConversation(IRetrieveMessageRequest messageRequest);

		[OperationContract]
		List<PostedMessageInfo> GetMessagesSentToUser(IRetrieveMessageRequest messageRequest);
	}
}