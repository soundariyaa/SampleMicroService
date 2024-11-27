using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleMicroService.Api.Extensions;
using SampleMicroService.Api.ResponseHelper;
using SampleMicroService.Application.Contracts;
using SampleMicroService.Application.Exceptions;

namespace SampleMicroService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController(
ISender mediator,
IProblemDetailsManager problemDetailsManager,
ILogger<BaseController> logger) 
    : ControllerBase
{
protected readonly ISender Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
protected readonly IProblemDetailsManager ProblemDetailsManager = problemDetailsManager ?? throw new ArgumentNullException(nameof(problemDetailsManager));
protected readonly ILogger<BaseController> Logger = logger ?? throw new ArgumentNullException(nameof(logger));

protected async Task<ObjectResult> SendMediatorAsync<TRequest, TResponse>(TRequest request)
    where TResponse : ResponseBase
    where TRequest : IRequest<TResponse>
{
    ProblemDetails? problemDetails;
    try
    {
        var response = await Mediator.Send(request, HttpContext.RequestAborted);
        problemDetails = response.LogProblemDetailsOnFailure(Logger);

        var status = problemDetails?.Status ?? StatusCodes.Status200OK;
        HttpContext.Response.StatusCode = status;
        return StatusCode(status, problemDetails == null ? response : problemDetails);
    }
    //catch (BusinessException ex)
    //{
    //    problemDetails = CreateProblemDetailsFromException(ex, context: ControllerContext);
    //}
    //catch (ValidationException ex)
    //{
    //    problemDetails = CreateProblemDetailsFromException(ex, context: ControllerContext);
    //}
    catch (Exception ex) when (ex is TaskCanceledException or OperationCanceledException)
    {
        problemDetails = CreateProblemDetailsFromException(ex, context: ControllerContext);
    }
    catch (UnhandledException ex)
    {
        problemDetails = CreateProblemDetailsFromException(ex, context: ControllerContext);
    }
    catch (Exception ex)
    {
        problemDetails = CreateProblemDetailsFromException(ex, context: ControllerContext);
    }
    HttpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status200OK;

    return StatusCode(problemDetails.Status ?? StatusCodes.Status200OK, problemDetails);
}

private ProblemDetails CreateProblemDetailsFromException<TException>(TException ex, ActionContext context) where TException : Exception
{
    var problemDetails = ProblemDetailsManager.GenerateProblemDetailsFromException(ex);
    //problemDetails.Instance = context.HttpContext.Request.PathAsString();
    return problemDetails;
}
}

