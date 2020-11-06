using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDbCore.Enumerations
{
	public enum QueryFailReason
	{
		PrimaryKeyDoesNotMatch = 1,
		UniqueColumnDoesNotMatch = 2,
		CouldNotFindRow = 3,
	}
}