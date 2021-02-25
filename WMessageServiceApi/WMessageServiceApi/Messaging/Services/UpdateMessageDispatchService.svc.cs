using System;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Logging;
using WMessageServiceApi.Messaging.ServiceBusinessLogics;
using WMessageServiceApi.Messaging.ServiceInterfaces;

namespace WMessageServiceApi.Messaging.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UpdateMessageDispatchService" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select UpdateMessageDispatchService.svc or UpdateMessageDispatchService.svc.cs at the Solution Explorer and start debugging.
	public class UpdateMessageDispatchService : IUpdateMessageDispatchService
    {
        public void UpdateDispatchAsReceived(long dispatchId, DateTime receivedDateTime)
        {
            try
            {
                UpdateMessageDispatchServiceBl.UpdateDispatchAsReceived(dispatchId, receivedDateTime);
            }
            catch (Exception exception)
            {
                ErrorContract error = new ErrorContract(exception.Message, StatusList.PROCESS_ERROR);
                LogError("Error encountered when executing Update-dispatch-As-Received.", exception);
                throw new FaultException<ErrorContract>(error);
            }
        }

        private void LogError(string message, Exception exception)
        {
            AppLog.LogError(message + "\n" + exception.ToString());
        }
    }
}