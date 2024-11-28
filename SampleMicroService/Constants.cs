namespace SampleMicroService.Api;

public static class Constants
{
    public static class Settings
    {
        public static string AppSettingsName => "appsettings";
        public static string AppSettingsJsonName => "appsettings.json";
    }
    public static class EnvironmentVariables
    {
        public static string AspNetCoreEnvironment => "ASPNETCORE_ENVIRONMENT";
        public const string AspNetVersion = "ASPNET_VERSION";
        public static string ElasticsearchUrl => "ELASTICSEARCH_URL";
        public static string ExecutionEnvironmentIdentifier => "EXECUTION_ENVIRONMENT_IDENTIFIER";
    }
}