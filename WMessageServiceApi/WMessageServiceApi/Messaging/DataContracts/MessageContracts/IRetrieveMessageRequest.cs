using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
	public interface IRetrieveMessageRequest
	{
		string AccessToken { get; set; }
	}
}