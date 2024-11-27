using Microsoft.AspNetCore.Mvc;
using SampleMicroService.Application.Contracts;
using SampleMicroService.Core.Models;

namespace SampleMicroService.Api.Contracts;

public sealed class ProductResponse : ResponseBase
{
    public ProductResponse()  => Status = StatusCodes.Status200OK;
    public ProductResponse(ProblemDetails problemDetails) : base(problemDetails) { }
    public IEnumerable<ProductSpecifications> Products { get; set; } = [];
}
