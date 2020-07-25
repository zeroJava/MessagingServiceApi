using System;
using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.EntityClasses;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using MessageDbLib.Logging;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
	public class UpdateMessageDispatchServiceBL
	{
		public void UpdateDispatchAsReceived(long dispatchId, DateTime receivedDateTime)
		{
			WriteInfoLog(string.Format("Recieved dispatch update request for dispatch (id): {0}.", dispatchId));

			IMessageDispatchRepository dispatchRepo = GetDispatchRepository();
			MessageDispatch dispatch = dispatchRepo.GetDispatchMatchingId(dispatchId);
			if (dispatch != null)
			{
				WriteInfoLog(string.Format("Found Dispatch (ID): {0}", dispatchId));

				dispatch.MessageReceived = true;
				dispatch.MessageReceivedTime = receivedDateTime;
				UpdateDispatch(dispatch);
			}
		}

		private IMessageDispatchRepository GetDispatchRepository()
		{
			return MessageDispatchRepoFactory.GetDispatchRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
		}

		private void UpdateDispatch(MessageDispatch dispatch)
		{
			long dispatchId = dispatch != null ? dispatch.Id : 0L;
			WriteInfoLog(string.Format("Going to update dispatch (ID): {0}", dispatchId));

			IMessageDispatchRepository dispatchRepo = GetDispatchRepository();
			dispatchRepo.UpdateDispatch(dispatch);
			WriteInfoLog(string.Format("Dispatch-object: {0} was sucessfully updated.", dispatchId));
		}

		private void WriteErrorLog(string message, Exception exception)
		{
			LogFile.WriteErrorLog(message, exception);
		}

		private void WriteInfoLog(string message)
		{
			LogFile.WriteInfoLog(message);
		}
	}
}