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

		public static double GetDouble(this DbDataReader reader, string key)
		{
			double value = 0;
			try
			{
				int ordinal = reader.GetOrdinal(key);
				return !reader.IsDBNull(ordinal) ? reader.GetDouble(ordinal) :
					value;
			}
			catch { return value; }
		}

		public static double? GetNlDouble(this DbDataReader reader, string key)
		{
			double? value = null;
			try
			{
				int ordinal = reader.GetOrdinal(key);
				return !reader.IsDBNull(ordinal) ? reader.GetDouble(ordinal) :
					value;
			}
			catch { return value; }
		}

		public static DateTime? GetNlDateTime(this DbDataReader reader, string key)
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

		public static DateTime GetDateTime(this DbDataReader reader, string key)
		{
			DateTime value = new DateTime();
			try
			{
				int ordinal = reader.GetOrdinal(key);
				return !reader.IsDBNull(ordinal) ? reader.GetDateTime(ordinal) :
					value;
			}
			catch { return value; }
		}

		public static Guid GetGuid(this DbDataReader reader, string key)
		{
			Guid value = default;
			try
			{
				int ordinal = reader.GetOrdinal(key);
				return !reader.IsDBNull(ordinal) ? reader.GetGuid(ordinal) :
					value;
			}
			catch { return value; }
		}

		public static bool GetBoolean(this DbDataReader reader, string key)
		{
			bool value = default;
			try
			{
				int ordinal = reader.GetOrdinal(key);
				return !reader.IsDBNull(ordinal) ? reader.GetBoolean(ordinal) :
					value;
			}
			catch { return value; }
		}

		public static bool? GetNlBoolean(this DbDataReader reader, string key)
		{
			bool? value = null;
			try
			{
				int ordinal = reader.GetOrdinal(key);
				return !reader.IsDBNull(ordinal) ? reader.GetBoolean(ordinal) :
					value;
			}
			catch { return value; }
		}
	}
}