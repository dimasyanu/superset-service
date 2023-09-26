namespace SupersetService.Extensions
{
    public static class ConfigurationExtension
    {
        public static string AppSetting(this IConfiguration config, string key)
            => config.GetSection("AppSettings").GetValue<string>(key);
    }
}
