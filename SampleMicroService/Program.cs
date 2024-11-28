using Microsoft.AspNetCore.Hosting;
using SampleMicroService.Api.Extensions;

namespace SampleMicroService.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddMediatRConfigurations();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    ////private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
    ////    .AddAllConfigurations()
    ////    .Build();
    //public static void Main(string[] args)
    //{
    //    var hostBuilder = CreateHostBuilder(args);

    //    hostBuilder
    //        .Build()
    //        .ApplyMigrations()
    //        .Run();

    //    //var hostBuilder = CreateHostBuilder(args);

    //    //new SerilogLoggingInitializerFactory()
    //    //    .Create(Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.ElasticsearchUrl) ?? string.Empty)
    //    //    .Initialize(hostBuilder, Configuration);

    //    //hostBuilder.Build().Run();
    //}

    //public static IHostBuilder CreateHostBuilder(string[] args) =>
    //    Host.CreateDefaultBuilder(args)
    //        .ConfigureWebHostDefaults(webBuilder =>
    //        {
    //            webBuilder.UseStartup<Startup>();
    //        });
}