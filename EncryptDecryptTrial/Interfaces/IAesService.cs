namespace EncryptDecryptTrial.Interfaces
{
	public interface IAesService
	{
		Task<byte[]> EncryptAsync(string clearText);
		Task<string> DecryptAsync(byte[] encrypted);
	}
}
