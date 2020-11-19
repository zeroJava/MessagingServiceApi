using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
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
