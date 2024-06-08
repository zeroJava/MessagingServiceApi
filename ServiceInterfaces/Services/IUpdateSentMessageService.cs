using System;
using System.ServiceModel;

namespace MessagingServiceInterfaces.Services
{
	[ServiceContract]
	public interface IUpdateSentMessageService
	{
		[OperationContract]
		void UpdateMessageAsReceived(long dispatchId, DateTime receivedDateTime);
	}
}