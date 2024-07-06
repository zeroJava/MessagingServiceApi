using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.DbRepositoryFactories;
using System;

namespace MessagingServiceFunctions.Message
{
	public class MessageDispatchUpdater : FunctionBase
	{
		private readonly IMessageDispatchRepository dispatchRepository;

		public MessageDispatchUpdater()
		{
			dispatchRepository = GetDispatchRepository();
		}

		private IMessageDispatchRepository GetDispatchRepository()
		{
			return MessageDispatchRepoFactory.GetDispatchRepository(engine,
				connectionString);
		}

		public void UpdateDispatchAsReceived(long dispatchId,
			DateTime receivedDateTime)
		{
			MessageDispatch dispatch =
				dispatchRepository.GetDispatch(dispatchId);
			if (dispatch != null)
			{
				dispatch.MessageReceived = true;
				dispatch.MessageReceivedTime = receivedDateTime;
				dispatchRepository.UpdateDispatch(dispatch);
			}
		}
	}
}