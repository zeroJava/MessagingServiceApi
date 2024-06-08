using MessagingServiceInterfaces.IContracts.Errors;
using System.Runtime.Serialization;

namespace MessagingServiceInterfaces.Contracts.Errors
{
	[DataContract(Name = "Error")]
	public class Error : IError
	{
		[DataMember(Name = "Message")]
		public string Message { get; set; }

		[DataMember(Name = "Status")]
		public int Status { get; set; }

		public Error()
		{
			//
		}

		public Error(string message)
		{
			Message = message;
		}

		public Error(string message, int status)
		{
			Message = message;
			Status = status;
		}
	}
}