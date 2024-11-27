using MongoDB.Driver;

namespace SampleMicroService.Infrastructure;

public static class Constants
{
    public static class MongoDb
    {
        public static string FindOptionsBatchSize => nameof(FindOptions.BatchSize);
        public static string FindOptionsNoCursorTimeout => nameof(FindOptions.NoCursorTimeout);
        public static string MongoClientSettingsRetryWrites => nameof(MongoClientSettings.RetryWrites);
    }
}
