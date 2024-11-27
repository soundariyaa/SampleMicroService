using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace SampleMicroService.Infrastructure.Repositories;

internal class ContextBase
{
    protected IMongoDatabase? _dataBase;
    protected readonly ILogger _logger;

    protected ContextBase(ILogger logger)
    {
        _logger=logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected virtual void InitializeMongoDBClient(DatabaseSettings databaseSettings)
    {
        try 
        {
            var mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(databaseSettings.ConnectionString));
            mongoClientSettings.SslSettings = new SslSettings { EnabledSslProtocols=SslProtocols.Tls12 };
            if (databaseSettings.Options.TryGetValue(Constants.MongoDb.MongoClientSettingsRetryWrites, out var retryWriteResult) &&
                retryWriteResult is bool retryWrites)
            {
                mongoClientSettings.RetryWrites = retryWrites;
            }

            var client = new MongoClient(mongoClientSettings);
            _dataBase = client.GetDatabase(databaseSettings.DatabaseName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{ErrorMessage}", ex.Message);
            throw;
        }
    }
}
