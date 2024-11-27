using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SampleMicroService.Core.Models;

namespace SampleMicroService.Infrastructure.Repositories;

internal sealed class ProductContext : ContextBase
{
    public ProductContext(IOptions<DatabaseSettings> settings, ILogger logger) : base(logger)
    { 
        ArgumentNullException.ThrowIfNull(nameof(settings));

    }

    public IMongoCollection<ProductSpecifications> Products => _dataBase!.GetCollection<ProductSpecifications>("clients")
        .WithReadPreference(new ReadPreference(ReadPreferenceMode.Nearest));
}
