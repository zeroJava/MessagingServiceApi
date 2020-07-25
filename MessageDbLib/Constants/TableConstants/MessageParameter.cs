using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDbLib.Constants.TableConstants
{
	public static class MessageParameter
	{
		public const string ID = "@messageId";
		public const string MESSAGE_TEXT = "@messageText";
		public const string SENDER_ID = "@senderId";
		public const string SENDER_EMAIL_ADDRESS = "@senderEmailAddress";
		public const string MESSAGE_CREATED = "@messageCreated";
		public const string MULTI_MEDIA_TYPE = "@multiMediaType";
		public const string UNIQUE_QUID = "@uniqueQuid";

		public const string FILE_NAME = "@fileName";
		public const string MEDIA_FILE_TYPE = "@mediaFileType";
		public const string FILE_SIZE = "@fileSize";
		public const string IMAGE_TYPE = "@imageType";
		public const string LENGTH = "@length";
	}
}