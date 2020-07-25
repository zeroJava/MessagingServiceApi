using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMessageServiceApi.Messaging.DataEnumerations;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
	public interface IMessageRequestTokenContract
	{
		long MessageId { get; set; }
		string Message { get; set; }
		MessageReceivedState MessageRecievedState { get; set; }
	}
}
