using MessageDbCore;
using MessageDbCore.RepoEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDbCore.DbRepositoryInterfaces
{
    public interface IMessageDispatchRepository : IDisposable
    {
        void InsertDispatch(MessageDispatch dispatch);
        void UpdateDispatch(MessageDispatch dispatch); // Tuple<string, IDbDataParameter[]> query where TParameter : IDbDataParameter;
        void DeleteDispatch(MessageDispatch dispatch);

        MessageDispatch GetDispatchMatchingId(long dispatchId);
        List<MessageDispatch> GetAllDispatches();
        List<MessageDispatch> GetDispatchesMatchingMessageId(long messageId);
        List<MessageDispatch> GetDispatchesMatchingEmail(string email);
        List<MessageDispatch> GetDispatchesNotReceivedMatchingEmail(string email);
        List<MessageDispatch> GetDispatchesBetweenSenderReceiver(string senderEmailAddress, string receiverEmailAddress,
            long messageIdThreshold, 
            int numberOfMessages);
    }
}