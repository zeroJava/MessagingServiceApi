using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMessageServiceApi.Messaging.DataContracts.UserContracts
{
	public interface IUpdateUserPasswordContract
	{
		string UserName { get; set; }
		string OldPassword { get; set; }
		string NewPassword { get; set; }
	}
}
