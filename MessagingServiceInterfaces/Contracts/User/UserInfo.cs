using MessagingServiceInterfaces.IContracts.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MessagingServiceInterfaces.Contracts.User
{
	[DataContract(Name = "UserInfo")]
	[KnownType(typeof(AdvancedUserInfo))]
	public class UserInfo : IUser
	{
		[Required]
		[StringLength(500)]
		[DataMember(Name = "UserName")]
		public string UserName { get; set; }

		[StringLength(500)]
		[DataMember(Name = "EmailAddress")]
		public string EmailAddress { get; set; }

		[StringLength(100)]
		[DataMember(Name = "FirstName")]
		public string FirstName { get; set; }

		[StringLength(100)]
		[DataMember(Name = "Surname")]
		public string Surname { get; set; }

		[DataMember(Name = "Dob")]
		public DateTime? Dob { get; set; }

		[StringLength(6)]
		[DataMember(Name = "Gender")]
		public string Gender { get; set; }
	}
}