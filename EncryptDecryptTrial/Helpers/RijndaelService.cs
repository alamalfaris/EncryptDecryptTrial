using System.Security.Cryptography;
using System.Text;

namespace EncryptDecryptTrial.Helpers
{
	public static class RijndaelService
	{
		private static readonly byte[] _key = Encoding.UTF8.GetBytes("0001028304051996");
		private static readonly byte[] _iv = Encoding.UTF8.GetBytes("1011121394152507");

		public static string Encrypt(string plainText)
		{
			return Convert.ToBase64String(EncryptStringToBytes(plainText));
		}

		public static string Decrypt(string cipherText)
		{
			var encrypted = Convert.FromBase64String(cipherText);
			return DecryptStringFromBytes(encrypted);
		}

		private static string DecryptStringFromBytes(byte[] cipherText)
		{
			// Check arguments.
			if (cipherText == null || cipherText.Length <= 0)
			{
				throw new ArgumentNullException("cipherText");
			}

			string plaintext = string.Empty;

			using (var rijAlg = new RijndaelManaged())
			{

				rijAlg.Mode = CipherMode.CBC;
				rijAlg.Padding = PaddingMode.PKCS7;
				rijAlg.FeedbackSize = 128;

				rijAlg.Key = _key;
				rijAlg.IV = _iv;


				var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
				try
				{
					using (var msDecrypt = new MemoryStream(cipherText))
					{
						using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
						{

							using (var srDecrypt = new StreamReader(csDecrypt))
							{
								plaintext = srDecrypt.ReadToEnd();

							}

						}
					}
				}
				catch
				{
					plaintext = "keyError";
				}
			}

			return plaintext;
		}


		private static byte[] EncryptStringToBytes(string plainText)
		{
			// Check arguments.
			if (plainText == null || plainText.Length <= 0)
			{
				throw new ArgumentNullException("plainText");
			}
			byte[] encrypted;
			// Create a RijndaelManaged object
			// with the specified key and IV.
			using (var rijAlg = new RijndaelManaged())
			{
				rijAlg.Mode = CipherMode.CBC;
				rijAlg.Padding = PaddingMode.PKCS7;
				rijAlg.FeedbackSize = 128;

				rijAlg.Key = _key;
				rijAlg.IV = _iv;

				// Create a decrytor to perform the stream transform.
				var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

				// Create the streams used for encryption.
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (var swEncrypt = new StreamWriter(csEncrypt))
						{
							//Write all data to the stream.
							swEncrypt.Write(plainText);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}

			// Return the encrypted bytes from the memory stream.
			return encrypted;
		}
	}
}
