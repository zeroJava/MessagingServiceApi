using System;
using System.Collections.Generic;

namespace MessagingServiceInterfaces.IContracts.Message
{
	public interface IMessageRequest
	{
		string AccessToken { get; set; }
		string UserName { get; set; }
		string Message { get; set; }
		List<string> EmailAccounts { get; set; }
		DateTime MessageCreated { get; set; }
	}
}