using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMessageServiceApi.Messaging.DataContracts.UserContracts
{
	public interface INewUserDataContract : IUserContract
	{
		string Password { get; set; }
	}
}
