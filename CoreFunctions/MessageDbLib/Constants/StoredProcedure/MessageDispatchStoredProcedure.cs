namespace MessageDbLib.Constants.StoredProcedure.Constants
{
   public class MessageDispatchStoredProcedure
   {
      public const string GetMessageDispatchesBetweenSenderReceiver = @"dbo.GetMessageDispatchesBetweenSenderReceiver";
      public const string GetMessagDispatchesBetweenSenderReceiverParameters = @"@senderEmailAddress, @recieverEmailAddress, @messageIdLimit, @messageCount";
   }
}
