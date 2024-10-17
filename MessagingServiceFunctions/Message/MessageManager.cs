using MessagingServiceInterfaces.Contracts.Message;
using MessagingServiceInterfaces.IContracts.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingServiceFunctions.Message
{
	/// <summary>
	/// The MessageManager class is responsible for managing various operations
	/// related to creating, retrieving, and updating messages.
	/// It acts as a mediator between different components like MessageCreator, 
	/// MessageRetriever and MessageDispatchUpdater
	/// </summary>
	public class MessageManager
	{
		protected readonly MessageCreator messageCreator;
		protected readonly MessageRetriever messageRetriever;
		protected readonly MessageDispatchUpdater messageDispatchUpdater;

		/// <summary>
		/// Default constructor that initializes the messageCreator field with a
		/// new instance of MessageCreator.
		/// </summary>
		public MessageManager()
		{
			this.messageCreator = new MessageCreator();
			this.messageRetriever = new MessageRetriever();
			this.messageDispatchUpdater = new MessageDispatchUpdater();
		}

		/// <summary>
		/// Overloaded constructor that accepts custom instances of 
		/// MessageCreator, MessageRetriever, and MessageDispatchUpdater. 
		/// If any of these parameters are null, new instances of those classes
		/// are created by default.
		/// </summary>
		/// <param name="messageCreator"></param>
		/// <param name="messageRetriever"></param>
		/// <param name="messageDispatchUpdater"></param>
		public MessageManager(MessageCreator messageCreator,
			MessageRetriever messageRetriever,
			MessageDispatchUpdater messageDispatchUpdater)
		{
			this.messageCreator = messageCreator ?? new MessageCreator();
			this.messageRetriever = messageRetriever ?? new MessageRetriever();
			this.messageDispatchUpdater = messageDispatchUpdater ?? new MessageDispatchUpdater();
		}

		public MessageRequestToken Create(IMessageRequest request)
		{
			return messageCreator.Create(request);
		}

		public List<PostedMessageInfo> GetMessagesSentToUser(IRetrieveMessageRequest messageRequest)
		{
			return messageRetriever.GetMessagesSentToUser(messageRequest);
		}

		public List<PostedMessageInfo> GetConversation(IRetrieveMessageRequest messageRequest)
		{
			return messageRetriever.GetConversation(messageRequest);
		}

		public void UpdateDispatchAsReceived(long dispatchId,
			DateTime receivedDateTime)
		{
			messageDispatchUpdater.UpdateDispatchAsReceived(dispatchId,
				receivedDateTime);
		}
	}
}