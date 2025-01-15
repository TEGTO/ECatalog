namespace ProductApi
{
    public static class ConfigurationKeys
    {
        public static string EF_CREATE_DATABASE { get; } = "EFCreateDatabase";
        public static string DATABASE_CONNECTION_STRING { get; } = "Db";
        public static string ALLOWED_CORS_ORIGINS { get; } = "AllowedCORSOrigins";
    }
}
