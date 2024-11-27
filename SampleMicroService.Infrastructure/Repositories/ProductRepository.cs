using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SampleMicroService.Core.Interfaces;
using MongoDB.Driver;
using SampleMicroService.Core.Models;
using MongoDB.Bson;

namespace SampleMicroService.Infrastructure.Repositories;

internal sealed class ProductRepository(
    IOptions<DatabaseSettings> settings,
    ILoggerFactory loggerFactory)
    : IProductRepository
{
    private readonly ProductContext _context = new(settings, loggerFactory.CreateLogger<ProductContext>());

    private readonly ILogger<ProductRepository> _logger = loggerFactory.CreateLogger<ProductRepository>();
    public async Task<IEnumerable<ProductSpecifications>> AllAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _context.Products.Find(x => x.Id != null).ToListAsync(cancellationToken);
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "Error occurred in {MethodName}", nameof(AllAsync));
            throw new Exception(string.Concat(ex.Message, $"Error occurred in {nameof(AllAsync)}"));
        }
    }
    public async Task<IEnumerable<ProductSpecifications>> SearchAsync(ProductSpecifications productSpecifications, CancellationToken cancellationToken)
    {
        try
        {
            var queryFilter = CreateFilter(productSpecifications);
            return await _context.Products.Find(queryFilter).ToListAsync(cancellationToken);
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "Error occurred in {MethodName}", nameof(SearchAsync));
            throw new Exception(string.Concat(ex.Message, $"Error occurred in {nameof(SearchAsync)}"));
        }
    }
    private FilterDefinition<ProductSpecifications> CreateFilter(ProductSpecifications productSpecifications)
    {
        var queryBuilder = Builders<ProductSpecifications>.Filter;
        var listOfFilters = new List<FilterDefinition<ProductSpecifications>>();
        if (productSpecifications.Name != null)
        {
            var filterClientName = queryBuilder.Regex(x => x.Name, BsonRegularExpression.Create($"/{productSpecifications.Name}/i"));
            listOfFilters.Add(filterClientName);
        }

        if (listOfFilters.Count == 0)
        {
            var filterAll = queryBuilder.Ne(x => x.Id, null);
            listOfFilters.Add(filterAll);
            _logger.LogInformation("No search criteria provided. Fetching all Clients.");
        }
        return queryBuilder.And(listOfFilters);
    }

    public async Task<bool> RemoveById(string id, CancellationToken cancellationToken)
    {
        try
        {
            var filterDefinition = Builders<ProductSpecifications>.Filter.Eq(client => client.Id, id);
            var result = await _context.Products.DeleteOneAsync(filterDefinition, cancellationToken);
            return result.DeletedCount != 0;
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "Error occurred in {MethodName}", nameof(RemoveById));
            throw new Exception(string.Concat(ex.Message, $"Error occurred in {nameof(RemoveById)}"));
        }
    }
    public async Task<string?> Save(string id, ProductSpecifications item, CancellationToken cancellationToken)
    {
        try
        {
            id ??= ObjectId.GenerateNewId().ToString();

            var opts = new UpdateOptions
            {
                IsUpsert = true,
                BypassDocumentValidation = false
            };
            var filter = Builders<ProductSpecifications>.Filter.Eq(product => product.Id, id);
            var update = Builders<ProductSpecifications>.Update
                .Set(product => product.Name, item.Name)
                .Set(product => product.Category, item.Category)
                .Set(product => product.Summary, item.Summary)
                .Set(product => product.Description, item.Description)
                .Set(product => product.ImageFile, item.ImageFile)
                .Set(product => product.Price, item.Price);

            var updateResult = await _context.Products.UpdateOneAsync(filter, update, opts, cancellationToken: cancellationToken);

            _logger.LogDebug("UpdateResult: {@UpdateResult}", new
            {
                updateResult.MatchedCount,
                updateResult.ModifiedCount,
                UpsertedId = updateResult.UpsertedId?.ToString()
            });
            return updateResult.UpsertedId?.ToString();
        }
        //catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        //{
        //    _logger.LogError(ex, "Error occurred in {MethodName}", nameof(Save));
        //   // throw new AlreadyExistsException("Hostname already exists");
        //}
        catch (MongoException ex)
        {
            _logger.LogError(ex, "Error occurred in {MethodName}", nameof(Save));
            throw new Exception(string.Concat(ex.Message, $"Error occurred in {nameof(Save)}"));
        }
    }

}
