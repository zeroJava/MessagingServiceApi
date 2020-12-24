using System;
using System.ServiceModel;

namespace WMessageServiceApi.Messaging.ServiceInterfaces
{
    [ServiceContract]
    public interface IUpdateMessageDispatchService
    {
        [OperationContract]
        void UpdateDispatchAsReceived(long dispatchId, DateTime receivedDateTime);
    }
}
