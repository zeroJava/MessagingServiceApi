using MessagingServiceApi.Messaging.ServiceLogics;
using MessagingServiceInterfaces.Contracts.Message;
using MessagingServiceInterfaces.Services;

namespace MessagingServiceApi.Messaging.Services
{
	public class CreateMessageService : ServiceBase, ICreateMessageService
	{
		public MessageRequestToken CreateMessage(MessageRequest message)
		{
			CreateMessageServiceLogic serviceHelper = new CreateMessageServiceLogic();
			return serviceHelper.CreateMessage(GetToken(), message);
		}
	}
}