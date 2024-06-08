using MessagingServiceInterfaces.IContracts.Login;
using System.Runtime.Serialization;

namespace MessagingServiceInterfaces.Contracts.Login
{
	[DataContract]
	public class LoginToken : ILoginToken
	{
		[DataMember]
		public bool LoginSuccessful { get; set; }

		[DataMember]
		public string UserName { get; set; }

		[DataMember]
		public string UserEmailAddress { get; set; }
		//[DataMember]
		//public User User { get; set; }
	}
}