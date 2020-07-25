using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WMessageServiceApi.Messaging.ServiceInterfaces
{
	[ServiceContract]
	public interface IUpdateMessageDispatchService
	{
		[OperationContract]
		void UpdateDispatchAsReceived(long dispatchId, DateTime receivedDateTime);
	}
}
