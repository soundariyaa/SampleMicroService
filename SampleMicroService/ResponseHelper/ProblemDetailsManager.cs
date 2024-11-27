using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SampleMicroService.Application.Exceptions;
//using SampleMicroService.Core.Exceptions;
using ValidationException = FluentValidation.ValidationException;

namespace SampleMicroService.Api.ResponseHelper;

public sealed class ProblemDetailsManager(string documentationUri) : IProblemDetailsManager
{
    private readonly string? _instance = string.IsNullOrWhiteSpace(documentationUri) ? null : documentationUri;

    private static readonly Dictionary<int, string> HttpStatusCodeTypeProvider = new()
    {
        {StatusCodes.Status200OK, "https://tools.ietf.org/html/rfc7231#section-6.3.1"},
        {StatusCodes.Status202Accepted, "https://tools.ietf.org/html/rfc7231#section-6.3.3"},
        {StatusCodes.Status400BadRequest, "https://tools.ietf.org/html/rfc7231#section-6.5.1"},
        {StatusCodes.Status401Unauthorized, "https://tools.ietf.org/html/rfc7235#section-3.1"},
        {StatusCodes.Status403Forbidden, "https://tools.ietf.org/html/rfc7231#section-6.5.3"},
        {StatusCodes.Status404NotFound, "https://tools.ietf.org/html/rfc7231#section-6.5.4"},
        {StatusCodes.Status409Conflict, "https://tools.ietf.org/html/rfc7231#section-6.5.8"},
        {StatusCodes.Status422UnprocessableEntity, "https://tools.ietf.org/html/rfc4918#section-11.2"},
        {StatusCodes.Status429TooManyRequests, "https://tools.ietf.org/html/rfc6585#section-4"},
        {StatusCodes.Status500InternalServerError, "https://tools.ietf.org/html/rfc7231#section-6.6.1"},
        {StatusCodes.Status501NotImplemented, "https://tools.ietf.org/html/rfc7231#section-6.6.2"},
        {StatusCodes.Status502BadGateway, "https://tools.ietf.org/html/rfc7231#section-6.6.3"},
        {StatusCodes.Status503ServiceUnavailable, "https://tools.ietf.org/html/rfc7231#section-6.6.4"},
        {StatusCodes.Status504GatewayTimeout, "https://tools.ietf.org/html/rfc7231#section-6.6.5"}
    };

    private static class Codes
    {
        public static int NoErrors => 0;
        public static int Error10GeneralExternalSystemError => 10;
        public static int Error20BrokenCircuit => 20;
        public static int Error30ServiceUnavailable => 30;
        public static int Error40Unauthorized => 40;
        public static int Error50Forbidden => 50;
        public static int Error104IncorrectConfiguredUser => 104;
        public static int Error800DataStore => 800;
        public static int Error900Validation => 900;
        public static int Error1000UnhandledException => 1000;
        public static int Error1200BadRequest => 1200;
        public static int Error1300CommunicationFailure => 1300;
        public static int Error1400DataNotFound => 1400;
        public static int Error1800RequestCancellation => 1800;
        public static int Error1900ConstraintViolation => 1900;
    }

    private static string GetStatusTypeOrEmptyString(int httpStatusCode)
    {
        return HttpStatusCodeTypeProvider.GetValueOrDefault(httpStatusCode) ?? string.Empty;
    }

    public ProblemDetailsManager() : this(string.Empty) { }

    private IReadOnlyDictionary<int, ProblemDetails> KnownProblemDetails => new Dictionary<int, ProblemDetails>([

        CreateProblemDetails(title: "No Error", status: StatusCodes.Status200OK, code: Codes.NoErrors),
        CreateProblemDetails(title: "External System Error", status: StatusCodes.Status502BadGateway,
            code: Codes.Error10GeneralExternalSystemError),
        CreateProblemDetails(title: "Circuit Breaker Open", status: StatusCodes.Status503ServiceUnavailable,
            code: Codes.Error20BrokenCircuit),
        CreateProblemDetails(title: "Service Unavailable", status: StatusCodes.Status503ServiceUnavailable,
            code: Codes.Error30ServiceUnavailable),
        CreateProblemDetails(title: "Unauthorized", status: StatusCodes.Status401Unauthorized,
            code: Codes.Error40Unauthorized),
        CreateProblemDetails(title: "Forbidden", status: StatusCodes.Status403Forbidden, code: Codes.Error50Forbidden),
        CreateProblemDetails(title: "User is not correctly configured", status: StatusCodes.Status400BadRequest,
            code: Codes.Error104IncorrectConfiguredUser),
        CreateProblemDetails(title: "Datastore Error", status: StatusCodes.Status502BadGateway,
            code: Codes.Error800DataStore),
        CreateProblemDetails(title: "Validation Error", status: StatusCodes.Status400BadRequest,
            code: Codes.Error900Validation),
        CreateProblemDetails(title: "Constraint Violation Error", status: StatusCodes.Status409Conflict,
            code: Codes.Error1900ConstraintViolation),
        CreateProblemDetails(title: "Unhandled Exception", status: StatusCodes.Status500InternalServerError,
            code: Codes.Error1000UnhandledException),
        CreateProblemDetails(title: "Bad Request", status: StatusCodes.Status400BadRequest,
            code: Codes.Error1200BadRequest),
        CreateProblemDetails(title: "Communication Failure", status: StatusCodes.Status502BadGateway,
            code: Codes.Error1300CommunicationFailure),
        CreateProblemDetails(title: "Data Not Found", status: StatusCodes.Status404NotFound,
            code: Codes.Error1400DataNotFound),
        CreateProblemDetails(
            title: "Request execution canceled. " +
                   "Either due to global timeout expiration before execution completion or caller terminating executing thread",
            status: StatusCodes.Status503ServiceUnavailable,
            code: Codes.Error1800RequestCancellation)
    ]);

    public ProblemDetails GenerateProblemDetailsFromException(Exception ex)
    {
        switch (ex)
        {
            case ValidationException validationException:
                {
                    var problemDetails = KnownProblemDetails[Codes.Error1200BadRequest];
                    problemDetails.Detail = validationException.Message;
                    problemDetails.Extensions["errors"] = validationException.Errors.Select(x => x.ErrorMessage).ToArray();
                    return problemDetails;
                }
            //case BusinessException businessException:
            //    {
            //        var problemDetails = KnownProblemDetails[Codes.Error1200BadRequest];
            //        problemDetails.Detail = businessException.Message;
            //        return problemDetails;
            //    }
            case AlreadyExistsException alreadyExistsException:
                {
                    var problemDetails = KnownProblemDetails[Codes.Error1900ConstraintViolation];
                    problemDetails.Detail = alreadyExistsException.Message;
                    return problemDetails;
                }
            //case NotFoundException notFoundException:
            //    {
            //        var problemDetails = KnownProblemDetails[Codes.Error1400DataNotFound];
            //        problemDetails.Detail = notFoundException.Message;
            //        problemDetails.Extensions["entityIdentifier"] = $"{notFoundException.EntityType}:{notFoundException.EntityId}";
            //        return problemDetails;
            //    }
            case UnhandledException unhandledException:
                {
                    var problemDetails = KnownProblemDetails[Codes.Error1000UnhandledException];
                    problemDetails.Detail = unhandledException.Message;
                    return problemDetails;
                }
            case TaskCanceledException taskCanceledException:
                {
                    var problemDetails = KnownProblemDetails[Codes.Error1800RequestCancellation];
                    problemDetails.Detail = taskCanceledException.Message;
                    return problemDetails;
                }
            case OperationCanceledException operationCanceledException:
                {
                    var problemDetails = KnownProblemDetails[Codes.Error1800RequestCancellation];
                    problemDetails.Detail = operationCanceledException.Message;
                    return problemDetails;
                }
            //case ExternalSystemException externalSystemException:
            //    {
            //        var problemDetails = KnownProblemDetails[Codes.Error10GeneralExternalSystemError];
            //        problemDetails.Detail = externalSystemException.Message;
            //        return problemDetails;
            //    }
            //case NasCredentialException nasCredentialException:
            //    {
            //        var problemDetails = KnownProblemDetails[Codes.Error1900ConstraintViolation];
            //        problemDetails.Detail = nasCredentialException.Message;
            //        return problemDetails;
            //    }
            case InvalidJsonException invalidJsonException:
                {
                    var problemDetails = KnownProblemDetails[Codes.Error1200BadRequest];
                    problemDetails.Detail = invalidJsonException.Message;
                    return problemDetails;
                }
            default:
                return KnownProblemDetails[Codes.Error1000UnhandledException];
        }
    }

    private KeyValuePair<int, ProblemDetails> CreateProblemDetails(string title, int status, int code) =>
        new(code, new ProblemDetails
        {
            Title = title,
            Status = status,
            Type = GetStatusTypeOrEmptyString(status),
            Instance = _instance,
            Extensions = new Dictionary<string, object?>
            {
                {"code", code}
            }
        });
}
