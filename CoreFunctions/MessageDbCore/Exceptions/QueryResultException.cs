using MessageDbCore.Enumerations;
using System;

namespace MessageDbCore.Exceptions
{
	public class QueryResultException : Exception
	{
		public QueryFailReason FailReason { get; private set; }

		public QueryResultException(string message, QueryFailReason failReason) : base(message)
		{
			FailReason = failReason;
		}

		public QueryResultException(string message, QueryFailReason failReason,
			Exception innerException) : base(message, innerException)
		{
			FailReason = failReason;
		}
	}
}