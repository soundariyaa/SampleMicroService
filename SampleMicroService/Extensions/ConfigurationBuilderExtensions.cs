namespace SampleMicroService.Api.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddAllConfigurations(this ConfigurationBuilder configBuilder)
    {
        return configBuilder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Constants.Settings.AppSettingsJsonName, false, true)
            .AddJsonFile(
                $"{Constants.Settings.AppSettingsName}.{Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.AspNetCoreEnvironment)}.json", true, true)
            .AddEnvironmentVariables();
    }

    //public static IApplicationBuilder UseEnvironmentDependentHsts(this IApplicationBuilder app, IWebHostEnvironment env, ILogger logger)
    //{
    //    if (!env.IsProduction())
    //    {
    //        logger.LogInformation("UseHsts not enabled for development environment: {EnvironmentName}", env.EnvironmentName);
    //        return app;
    //    }
    //    logger.LogInformation("UseHsts enabled for none development environment: {EnvironmentName}", env.EnvironmentName);
    //    app.UseHsts();
    //    return app;
    //}
}
