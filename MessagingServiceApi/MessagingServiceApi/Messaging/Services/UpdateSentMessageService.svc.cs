using MessagingServiceInterfaces.Services;
using System;

namespace MessagingServiceApi.Messaging.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UpdateSentMessageService" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select UpdateSentMessageService.svc or UpdateSentMessageService.svc.cs at the Solution Explorer and start debugging.
	public class UpdateSentMessageService : ServiceBase, IUpdateSentMessageService
	{
		public void UpdateMessageAsReceived(long dispatchId, DateTime receivedDateTime)
		{
			var serviceHelper = new ServiceLogics.UpdateSentMessageService();
			serviceHelper.UpdateDispatchAsReceived(GetToken(), dispatchId,
				receivedDateTime);
		}
	}
}