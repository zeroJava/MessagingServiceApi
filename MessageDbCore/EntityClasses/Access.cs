using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDbCore.EntityClasses
{
	public class Access
	{
		public long Id { get; set; }
		public string Organisation { get; set; }
		public string Token { get; set; }
		public long UserId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string[] Scope { get; set; }
	}
}