using System.Runtime.Serialization;

namespace WMessageServiceApi.Exceptions.Datacontacts
{
	[DataContract(Name = "UserExistErrorContract")]
	public class UserExistErrorContract : IErrorsContract
	{
		[DataMember(Name = "Message")]
		public string Message { get; set; }

		[DataMember(Name = "Status")]
		public int Status { get; set; }
	}
}