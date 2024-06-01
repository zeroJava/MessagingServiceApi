using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.ServiceHelpers;
using WMessageServiceApi.Messaging.ServiceInterfaces;

namespace WMessageServiceApi.Messaging.Services
{
	public class CreateMessageService : BaseService, ICreateMessageService
	{
		public MessageRequestToken CreateMessage(MessageRequest message)
		{
			CreateMessageServiceHelper serviceHelper = new CreateMessageServiceHelper();
			return serviceHelper.CreateMessage(GetToken(), message);
		}
	}
}