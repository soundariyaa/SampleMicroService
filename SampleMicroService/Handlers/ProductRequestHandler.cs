using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SampleMicroService.Api.Contracts;
using SampleMicroService.Api.ResponseHelper;
using SampleMicroService.Core.Interfaces;
using SampleMicroService.Core.Models;

namespace SampleMicroService.Api.Handlers;

public class ProductRequestHandler
(
    IProductRepository productRepository,
    IProblemDetailsManager problemDetailsManager,
    IMapper mapper,
    ILogger<ProductRequestHandler> logger)
    : IRequestHandler<ProductRequest, ProductResponse>
{
    private readonly IProblemDetailsManager _problemDetailsManager = problemDetailsManager ?? throw new ArgumentNullException(nameof(problemDetailsManager));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly ILogger<ProductRequestHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<ProductResponse> Handle(ProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var productSpecification = _mapper.Map<ProductSpecifications>(request);
            var productResult = await productRepository.SearchAsync(productSpecification, cancellationToken);

            var productView = _mapper.Map<IEnumerable<ProductSpecifications>>(productResult);

            var result = new ProductResponse
            {
                Products = productView
            };

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling {MethodName} with ZonesRequest {@ZonesRequest}", nameof(Handle), request);

            return new ProductResponse(_problemDetailsManager.GenerateProblemDetailsFromException(ex));
        }
    }
}