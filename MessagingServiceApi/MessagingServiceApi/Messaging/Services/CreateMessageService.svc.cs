using MessagingServiceInterfaces.Contracts.Message;
using MessagingServiceInterfaces.Services;

namespace MessagingServiceApi.Messaging.Services
{
	public class CreateMessageService : ServiceBase, ICreateMessageService
	{
		public MessageRequestToken CreateMessage(MessageRequest message)
		{
			ServiceLogics.CreateMessageService serviceHelper =
				new ServiceLogics.CreateMessageService();
			return serviceHelper.CreateMessage(GetToken(), message);
		}
	}
}