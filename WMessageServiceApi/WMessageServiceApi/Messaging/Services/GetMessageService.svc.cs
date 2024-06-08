using MessagingServiceApi.Messaging.ServiceLogics;
using MessagingServiceInterfaces.Contracts.Message;
using MessagingServiceInterfaces.IContracts.Message;
using MessagingServiceInterfaces.Services;
using System.Collections.Generic;

namespace MessagingServiceApi.Messaging.Services
{
	public class GetMessageService : ServiceBase, IGetMessageService
	{
		public List<PostedMessageInfo> GetMessagesSentToUser(IRetrieveMessageRequest messageRequest)
		{
			GetMessageServiceLogic serviceHelper = new GetMessageServiceLogic();
			return serviceHelper.GetMessagesSentToUser(GetToken(), messageRequest);
		}

		public List<PostedMessageInfo> GetConversation(IRetrieveMessageRequest messageRequest)
		{
			GetMessageServiceLogic serviceHelper = new GetMessageServiceLogic();
			return serviceHelper.GetConveration(GetToken(), messageRequest);
		}
	}
}