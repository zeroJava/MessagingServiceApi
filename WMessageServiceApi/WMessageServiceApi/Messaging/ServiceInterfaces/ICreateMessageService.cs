using System.ServiceModel;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;

namespace WMessageServiceApi.Messaging.ServiceInterfaces
{
	[ServiceContract]
	public interface ICreateMessageService
	{
		[OperationContract]
		MessageRequestTokenContract CreateMessage(MessageRequest message);
	}
}
