using System;
using System.IO;
using System.Security.Cryptography;

namespace Cryptography
{
	public static class SymmetricEncryption
	{
		private static readonly byte[] key =
		{
			84, 104, 105, 115, 73, 115, 77, 121,
			67, 114, 121, 112, 16, 111, 80, 114,
		};

		private static readonly byte[] initialVector =
		{
			14, 194, 125, 200, 03, 191, 98, 102,
			27, 172, 201, 211, 16, 111, 10, 214,
		};

		public static string Encrypt(string valuestr)
		{
			if (string.IsNullOrEmpty(valuestr))
			{
				throw new ApplicationException("The string value is empyt.");
			}

			//byte[] valueByte = Convert.FromBase64String(valuestr);

			using (SymmetricAlgorithm symmetricAlgorithm = Aes.Create())
			using (ICryptoTransform encryptor = symmetricAlgorithm.CreateEncryptor(key, initialVector))
			using (MemoryStream encryptedStream = new MemoryStream())
			using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, encryptor,
				CryptoStreamMode.Write))
			{
				using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
				{
					streamWriter.Write(valuestr);
				}
				return Convert.ToBase64String(encryptedStream.ToArray());
			}
		}

		public static string Decrypt(string encrptValuestr)
		{
			if (string.IsNullOrEmpty(encrptValuestr))
			{
				throw new ApplicationException("The encrypted string value is empty.");
			}

			byte[] valueByte = Convert.FromBase64String(encrptValuestr);

			using (SymmetricAlgorithm symmetricAlgorithm = Aes.Create())
			using (ICryptoTransform decryptor = symmetricAlgorithm.CreateDecryptor(key, initialVector))
			using (MemoryStream encryptedStream = new MemoryStream(valueByte))
			using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, decryptor,
				CryptoStreamMode.Read))
			{
				using (StreamReader streamReader = new StreamReader(cryptoStream))
				{
					return streamReader.ReadToEnd();
				}
			}
		}
	}
}