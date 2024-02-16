using System.Collections.Generic;
using System.ServiceModel;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;

namespace WMessageServiceApi.Messaging.ServiceInterfaces
{
	[ServiceContract]
	[ServiceKnownType(typeof(RetrieveMessageRequest))]
	public interface IRetrieveMessageService
	{
		[OperationContract]
		List<MessageDispatchInfo> GetMessageDipatchesBetweenSenderReceiver(IRetrieveMessageRequest messageRequest);

		[OperationContract]
		List<MessageDispatchInfo> GetMessagesSentToUser(IRetrieveMessageRequest messageRequest);
	}
}