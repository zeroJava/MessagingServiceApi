using MessageDbCore.RepoEntity;
using System;
using System.Collections.Generic;

namespace MessageDbCore.DbRepositoryInterfaces
{
	public interface IMessageRepository : IDisposable
	{
		void InsertMessage(Message message);
		void UpdateMessage(Message message); // Tuple<string, TParameter[]> query where TParameter : IDbDataParameter;
		void DeleteMessage(Message message);

		Message GetMessage(long messageId);
		List<Message> GetMessagesMatchingText(string text);
		List<Message> GetMessages();
		List<Message> GetMessages(long[] messageids);
	}
}