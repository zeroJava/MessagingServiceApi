using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMessageServiceApi.Extensions
{
	public static class StringExtension
	{
		public static bool IsNotNullEmpty(this string value)
		{
			return value != null && value != string.Empty;
		}
	}
}