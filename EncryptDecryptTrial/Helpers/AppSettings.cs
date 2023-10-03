namespace EncryptDecryptTrial.Helpers
{
	public static class AppSettings
	{
		private static IConfiguration _config;
		public static void ConfigureSetting(IConfiguration config)
		{
			_config = config;
		}

		private static string Setting(string Key)
		{
			return RijndaelService.Decrypt(_config.GetSection(Key).Value);
		}

		static Type? GetSettingAsType<Type>(object obj, Func<object, Type> callerConverter)
		{

			if (obj != null)
			{
				Type? value;
				try
				{
					value = callerConverter(obj);
				}
				catch
				{
					value = default;
				}
				return value;
			}
			else
				return default;
		}

		public static int GetSettingAsInteger(string key)
		{
			return GetSettingAsType<int>(Setting(key), obj => Convert.ToInt32(obj));
		}
		public static string? GetSettingAsString(string key)
		{
			return GetSettingAsType(Setting(key), obj => Convert.ToString(obj));
		}
		public static bool? GetSettingAsBool(string key)
		{
			return GetSettingAsType<bool?>(Setting(key), obj => Convert.ToBoolean(obj));
		}
	}
}
