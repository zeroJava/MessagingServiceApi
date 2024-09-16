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
			var serviceHelper = new ServiceLogics.GetMessageService();
			return serviceHelper.GetMessagesSentToUser(GetToken(), messageRequest);
		}

		public List<PostedMessageInfo> GetConversation(IRetrieveMessageRequest messageRequest)
		{
			var serviceHelper = new ServiceLogics.GetMessageService();
			return serviceHelper.GetConveration(GetToken(), messageRequest);
		}
	}
}