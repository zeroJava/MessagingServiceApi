using System.Data.Common;

namespace MessageDbLib.Extensions
{
	internal static class DbDataReaderExtension
	{
		public static string GetStrValue(this DbDataReader reader, string key)
		{
            try
            {
				return reader?[key]?.ToString();
			}
            catch { return null; }
		}

		public static long GetLongValue(this DbDataReader reader, string key)
		{
			long value = default;
			try
            {
				return long.TryParse(reader?[key]?.ToString(), out value) ?
					value : default;
			}
            catch { return value; }
		}
	}
}