using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDbLib.Constants.StoredProcedure.Constants
{
    public class MessageStoredProcedure
    {
        public const string GetAllMessages = @"[messagedbo].[GetAllMessages]";

        public const string GetMessagesBetweenSenderReceiver = @"messagedbo.GetMessagesBetweenSenderReceiver";
        public const string GetMessagesBetweenSenderReceiverParameters = @"@senderEmailAddress, @recieverEmailAddress, @messageIdLimit, @messageCount";
    }
}