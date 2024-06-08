using MessagingServiceInterfaces.Contracts.Message;
using System.ServiceModel;

namespace MessagingServiceInterfaces.Services
{
	[ServiceContract]
	public interface ICreateMessageService
	{
		[OperationContract]
		MessageRequestToken CreateMessage(MessageRequest message);
	}
}