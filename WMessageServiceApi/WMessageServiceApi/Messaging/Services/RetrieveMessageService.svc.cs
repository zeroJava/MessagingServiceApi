using System.Collections.Generic;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.ServiceHelpers;
using WMessageServiceApi.Messaging.ServiceInterfaces;

namespace WMessageServiceApi.Messaging.Services
{
	public class RetrieveMessageService : BaseService, IRetrieveMessageService
	{
		public List<MessageDispatchInfo> GetMessagesSentToUser(IRetrieveMessageRequest messageRequest)
		{
			RetrieveMessageServiceHelper serviceHelper = new RetrieveMessageServiceHelper();
			return serviceHelper.GetMessagesSentToUser(GetToken(), messageRequest);
		}

		public List<MessageDispatchInfo> GetMessageDipatchesBetweenSenderReceiver(IRetrieveMessageRequest messageRequest)
		{
			RetrieveMessageServiceHelper serviceHelper = new RetrieveMessageServiceHelper();
			return serviceHelper.GetMessagesBetweenSenderReceiver(GetToken(), messageRequest);
		}
	}
}