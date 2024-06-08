namespace MessagingServiceApi.Extensions
{
	public static class StringExtension
	{
		public static bool IsNotNullEmpty(this string value)
		{
			return value != null && value != string.Empty;
		}
	}
}