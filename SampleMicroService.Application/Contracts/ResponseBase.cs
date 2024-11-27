using Microsoft.AspNetCore.Mvc;

namespace SampleMicroService.Application.Contracts;

public abstract class ResponseBase
{
    protected ResponseBase() { }
    protected ResponseBase(ProblemDetails problemDetails) 
    {
        ArgumentNullException.ThrowIfNull(problemDetails, nameof(problemDetails));
        Status = problemDetails.Status;
        Title = problemDetails.Title;
        Detail = problemDetails.Detail;
        Type = problemDetails.Type;
        Instance = problemDetails.Instance;
        Extensions = problemDetails.Extensions.Count == 0 ? null : problemDetails.Extensions;
    }
    public int? Status { get; set; }
    public string? Title { get; set; }
    public string? Detail { get; set; }
    public string? Type { get; set; }
    public string? Instance { get; set; }
    public IDictionary<string, object?>? Extensions { get; set; }
}
