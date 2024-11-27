using Microsoft.AspNetCore.Mvc;

namespace SampleMicroService.Api.ResponseHelper;

public interface IProblemDetailsManager
{
    ProblemDetails GenerateProblemDetailsFromException(Exception ex);

}
