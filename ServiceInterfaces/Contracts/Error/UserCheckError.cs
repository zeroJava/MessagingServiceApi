using MessagingServiceInterfaces.IContracts.Errors;
using System.Runtime.Serialization;

namespace MessagingServiceInterfaces.Contracts.Errors
{
	[DataContract(Name = "UserCheckError")]
	public class UserCheckError : IError
	{
		[DataMember(Name = "Message")]
		public string Message { get; set; }

		[DataMember(Name = "Status")]
		public int Status { get; set; }
	}
}