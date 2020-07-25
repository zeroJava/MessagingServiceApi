using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;

namespace WMessageServiceApi.Messaging.ServiceInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRetrieveMessageService" in both code and config file together.
    [ServiceContract]
    public interface IRetrieveMessageService
    {
        [OperationContract]
        List<MessageDispatchInfoContract> GetMessageDipatchesBetweenSenderReceiver(string username, string senderEmailAddress, string receiverEmailAddress, 
			long messageIdThreshold, int numberOfMessages);

		[OperationContract]
		List<MessageDispatchInfoContract> GetMessagesSentToUser(string username, string receiverEmailAddress);
    }
}
