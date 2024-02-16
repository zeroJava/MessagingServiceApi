using System.ServiceModel;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;

namespace WMessageServiceApi.Messaging.ServiceInterfaces
{
	[ServiceContract]
	public interface ICreateMessageService
	{
		[OperationContract]
		MessageRequestToken CreateMessage(MessageRequest message);
	}
}
