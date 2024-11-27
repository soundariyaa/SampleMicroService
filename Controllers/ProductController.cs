using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleMicroService.Api.Contracts;
using SampleMicroService.Api.Controllers;
using SampleMicroService.Api.ResponseHelper;

namespace SampleMicroService.Controllers;

public class ProductController(
    ISender mediator,
    IProblemDetailsManager problemDetailsManager,
    ILogger<ProductController> logger) : BaseController(mediator, problemDetailsManager, logger)
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<ProductResponse>> GetProductsById(
        [FromQuery] ProductRequest query)
        => await SendMediatorAsync<ProductRequest, ProductResponse>(query);

    [HttpPost]
    [ProducesResponseType(typeof(CreateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<CreateResponse>> CreateProduct(
       [FromBody] CreateProductRequest createProductRequest)
       => await SendMediatorAsync<CreateProductRequest, CreateResponse>(createProductRequest);
}
