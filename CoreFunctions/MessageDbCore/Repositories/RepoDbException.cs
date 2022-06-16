using System;
using System.Text;

namespace MessageDbCore.Repositories
{
	public class RepoDbException : Exception
	{
		public DateTime EventTimestamp { get; private set; }

		public RepoDbException(string message, Exception innerException) : base(message, innerException)
		{
			EventTimestamp = DateTime.Now;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(Message + " on " + EventTimestamp);
			stringBuilder.AppendLine(base.ToString());

			return stringBuilder.ToString();
		}
	}
}