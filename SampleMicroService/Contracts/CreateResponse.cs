using Microsoft.AspNetCore.Mvc;
using SampleMicroService.Application.Contracts;

namespace SampleMicroService.Api.Contracts
{
    public class CreateResponse : ResponseBase
    {
        public string? Id { get; init; }

        public CreateResponse()
        {
            Status = StatusCodes.Status201Created;
        }

        public CreateResponse(ProblemDetails problemDetails) : base(problemDetails)
        {

        }
    }
}
