using System;
using System.Data.Common;

namespace MessageDbLib.Extensions
{
	internal static class DbDataReaderExtension
	{
		public static string GetString(this DbDataReader reader, string key)
		{
			try
			{
				return reader?[key]?.ToString();
			}
			catch { return null; }
		}

		public static long GetInt64(this DbDataReader reader, string key)
		{
			long value = default;
			try
			{
				int ordinal = reader.GetOrdinal(key);
				return !reader.IsDBNull(ordinal) ? reader.GetInt64(ordinal) :
					value;
			}
			catch { return value; }
		}

		public static DateTime? GetDateTime(this DbDataReader reader, string key)
		{
			DateTime? value = null;
			try
			{
				int ordinal = reader.GetOrdinal(key);
				return !reader.IsDBNull(ordinal) ? reader.GetDateTime(ordinal) :
					value;
			}
			catch { return value; }
		}
	}
}