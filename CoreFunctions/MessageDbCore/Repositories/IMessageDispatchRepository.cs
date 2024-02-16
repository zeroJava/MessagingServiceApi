using MessageDbCore.RepoEntity;
using System;
using System.Collections.Generic;

namespace MessageDbCore.DbRepositoryInterfaces
{
	public interface IMessageDispatchRepository : IDisposable
	{
		void InsertDispatch(MessageDispatch dispatch);
		void UpdateDispatch(MessageDispatch dispatch); // Tuple<string, IDbDataParameter[]> query where TParameter : IDbDataParameter;
		void DeleteDispatch(MessageDispatch dispatch);

		MessageDispatch GetDispatch(long dispatchId);
		List<MessageDispatch> GetDispatches();
		List<MessageDispatch> GetDispatchesMessageId(long messageId);
		List<MessageDispatch> GetDispatchesEmail(string email);
		List<MessageDispatch> GetDispatchesNotReceived(string email);
		List<MessageDispatch> GetDispatchesSenderReceiver(string senderEmailAddress, string receiverEmailAddress,
			 long messageIdThreshold,
			 int numberOfMessages);
	}
}