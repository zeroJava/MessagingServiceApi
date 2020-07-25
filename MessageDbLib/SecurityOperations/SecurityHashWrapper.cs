using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MessageDbLib.SecurityOperations
{
	public static class SecurityHashWrapper
	{
		public static string Get512HashString(string sourceObject)
		{
			if (sourceObject == null && sourceObject == string.Empty)
			{
				throw new InvalidOperationException("The hashable object assigned is null");
			}

			SHA512Managed sha512Managed = new SHA512Managed();
			byte[] data = Encoding.UTF8.GetBytes(sourceObject);
			byte[] results = sha512Managed.ComputeHash(data);

			return Convert.ToBase64String(results);
		}
	}
}
