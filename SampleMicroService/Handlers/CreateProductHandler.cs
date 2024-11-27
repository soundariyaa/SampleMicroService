using AutoMapper;
using FluentValidation;
using MediatR;
using SampleMicroService.Api.Contracts;
using SampleMicroService.Api.ResponseHelper;
using SampleMicroService.Api.Validators;
using SampleMicroService.Core.Interfaces;
using SampleMicroService.Core.Models;

namespace SampleMicroService.Api.Handlers;

public sealed class CreateProductHandler(
    IProductRepository productRepository,
    IProblemDetailsManager problemDetailsManager,
         IMapper mapper,
    ILogger<CreateProductHandler> logger)
    : IRequestHandler<CreateProductRequest, CreateResponse>
{
    private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    private readonly IMapper _mapper= mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IProblemDetailsManager _problemDetailsManager = problemDetailsManager ?? throw new ArgumentNullException(nameof(problemDetailsManager));
    private readonly ILogger<CreateProductHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    public async Task<CreateResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await new CreateProductRequestValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = $"{request.GetType().Name} is not valid";
                throw new ValidationException(message, validationResult.Errors);
            }

            var productSpecification = _mapper.Map<ProductSpecifications>(request);

            var productId = await _productRepository.Save(productSpecification, cancellationToken);

            return new CreateResponse { Id = productId };
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "{ExceptionType} occurred while handling {RequestType}", ex.GetType().Name, request.GetType().Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling {RequestType}", nameof(ProductSpecifications));
            return new CreateResponse(_problemDetailsManager.GenerateProblemDetailsFromException(ex));
        }
    }
}
