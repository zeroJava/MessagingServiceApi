using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using System;
using WMessageServiceApi.Logging;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
	public static class UpdateMessageDispatchLogic
	{
		public static void UpdateDispatchAsReceived(long dispatchId, DateTime receivedDateTime)
		{
			WriteInfoLog($"Recieved dispatch update request for dispatch (id): {dispatchId}");

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

		private static IMessageDispatchRepository GetDispatchRepository()
		{
			return MessageDispatchRepoFactory.GetDispatchRepository(DatabaseOption.DatabaseEngine,
				 DatabaseOption.DbConnectionString);
		}

		private static void UpdateDispatch(MessageDispatch dispatch)
		{
			long dispatchId = dispatch != null ? dispatch.Id : 0L;
			WriteInfoLog(string.Format("Going to update dispatch (ID): {0}", dispatchId));

			IMessageDispatchRepository dispatchRepo = GetDispatchRepository();
			dispatchRepo.UpdateDispatch(dispatch);
			WriteInfoLog(string.Format("Dispatch-object: {0} was sucessfully updated.", dispatchId));
		}

		private static void WriteErrorLog(string message, Exception exception)
		{
			AppLog.LogError(message + "\n" + exception.ToString());
		}

		private static void WriteInfoLog(string message)
		{
			AppLog.LogInfo(message);
		}
	}
}