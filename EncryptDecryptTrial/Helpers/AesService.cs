using EncryptDecryptTrial.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace EncryptDecryptTrial.Helpers
{
	public class AesService : IAesService
	{
		private readonly byte[] _key = Encoding.UTF8.GetBytes("0001028304051996");
		private readonly byte[] _iv = Encoding.UTF8.GetBytes("1011121394152507");

		public async Task<byte[]> EncryptAsync(string clearText)
		{
			using Aes aes = Aes.Create();
			aes.Key = _key;
			aes.IV = _iv;

			using MemoryStream output = new();
			using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
			await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(clearText));
			await cryptoStream.FlushFinalBlockAsync();

			return output.ToArray();
		}

		public async Task<string> DecryptAsync(byte[] encrypted)
		{
			using Aes aes = Aes.Create();
			aes.Key = _key;
			aes.IV = _iv;

			using MemoryStream input = new(encrypted);
			using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
			using MemoryStream output = new();
			await cryptoStream.CopyToAsync(output);

			return Encoding.Unicode.GetString(output.ToArray());
		}
	}
}
