using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMessageServiceApi.Messaging.DataContracts.UserContracts
{
	public interface IUserContract
	{
		string UserName { get; set; }
		string EmailAddress { get; set; }
		string FirstName { get; set; }
		string Surname { get; set; }
		DateTime? Dob { get; set; }
		string Gender { get; set; }
	}
}
