using System.Collections.Generic;
using System.ServiceModel;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;

namespace WMessageServiceApi.Messaging.ServiceInterfaces
{
    [ServiceContract]
    [ServiceKnownType(typeof(RetrieveMessageRequest))]
    public interface IRetrieveMessageService
    {
        [OperationContract]
        List<MessageDispatchInfoContract> GetMessageDipatchesBetweenSenderReceiver(IRetrieveMessageRequest messageRequest);

        [OperationContract]
        List<MessageDispatchInfoContract> GetMessagesSentToUser(IRetrieveMessageRequest messageRequest);
    }
}