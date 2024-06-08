using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace MessagingServiceInterfaces.Services
{
	[ServiceContract]
	public interface ITestRestService
	{
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "PostTest")]
		void PostTest();

		[OperationContract]
		[WebInvoke(Method = "*", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "PostTestTwo")]
		void PostTestTwo(string number);

		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "PostTestThree")]
		void PostTestThree(string number, int intNumber);

		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "GetTest?number={number}")]
		RValue GetTest(int number);
	}

	[DataContract]
	public class RValue
	{
		[DataMember]
		public string Value { get; set; }
	}
}
