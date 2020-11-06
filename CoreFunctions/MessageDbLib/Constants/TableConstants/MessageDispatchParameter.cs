using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDbLib.Constants.TableConstants
{
	public sealed class MessageDispatchParameter
	{
		public const string ID = "@id";
		public const string EMAIL_ADDRESS = "@emailAddress";
		public const string MESSAGE_ID = "@messageId";
		public const string MESSAGE_RECEIVED = "@messageReceived";
		public const string MESSAGE_RECEIVED_TIME = "@messageReceivedTime";		
	}
}