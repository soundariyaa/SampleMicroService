using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SampleMicroService.Core.Interfaces;
using SampleMicroService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace SampleMicroService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions =
       new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNameCaseInsensitive = true };

        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration) => services
       .AddDatabaseConfigurations(configuration)
        .AddDatabaseRepository();
        private static IServiceCollection AddDatabaseConfigurations(this IServiceCollection services, IConfiguration configuration) => services
       .Configure<DatabaseSettings>(configure =>
       {
           configure.ConnectionString = configuration.GetSection(Core.Constants.EnvironmentVariables.MongoDbConnectionString).Value;
           configure.DatabaseName = configuration.GetSection(Core.Constants.EnvironmentVariables.MongoDbDatabaseName).Value;
           ArgumentNullException.ThrowIfNull(configure.ConnectionString, nameof(configure.ConnectionString));
           ArgumentNullException.ThrowIfNull(configure.DatabaseName, nameof(configure.DatabaseName));
       });

        private static IServiceCollection AddDatabaseRepository(this IServiceCollection services) => services
       .AddSingleton<IProductRepository, ProductRepository>();
    }
}
