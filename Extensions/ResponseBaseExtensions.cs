using Microsoft.AspNetCore.Mvc;
using SampleMicroService.Application.Contracts;

namespace SampleMicroService.Api.Extensions;

public static class ResponseBaseExtensions
{
    public static ProblemDetails? LogProblemDetailsOnFailure(this ResponseBase responseBase, ILogger logger, LogLevel logLevel = LogLevel.Debug)
    {
        if (responseBase == null ||
            responseBase.IsSuccessfulResponse() ||
            logger == null ||
            !logger.IsEnabled(logLevel)) { return null; }

        var problemDetails = new ProblemDetails
        {
            Status = responseBase.Status,
            Title = responseBase.Title,
            Detail = responseBase.Detail,
            Instance = responseBase.Instance,
            Type = responseBase.Type,
            Extensions = responseBase.Extensions.ToDictionary(pair => pair.Key, pair => pair.Value)
        };

        //logger.Log(logLevel, "@{ProblemDetails} {{{responseBase.GetType().Name}}}",
        //    problemDetails.MakeMeDestructible(), responseBase.GetType().Name);

        logger.Log(logLevel, "@{ProblemDetails@} {{{{responseBase.GetType().Name}}}");

        return problemDetails;
    }

    public static bool IsSuccessfulResponse(this ResponseBase responseBase)
    {
        const int status299OkUnassigned = 299;
        return
            responseBase?.Status >= StatusCodes.Status200OK &&
            responseBase.Status <= status299OkUnassigned;
    }
}
