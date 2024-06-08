using System;

namespace MessagingServiceInterfaces.IContracts.User
{
	public interface IUser
	{
		string UserName { get; set; }
		string EmailAddress { get; set; }
		string FirstName { get; set; }
		string Surname { get; set; }
		DateTime? Dob { get; set; }
		string Gender { get; set; }
	}
}