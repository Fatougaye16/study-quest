using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace StudyQuest.API.Extensions;

public static class ErrorOrResultExtensions
{
    public static IResult ToProblemResult(this List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Results.Problem(title: "An unknown error occurred.");
        }

        var areValidationErrors = errors.All(error => error.Type == ErrorType.Validation);
        if (areValidationErrors)
        {
            var validationProblems = errors
                .GroupBy(error => error.Code)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.Description).ToArray());

            return Results.ValidationProblem(
                errors: validationProblems,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Validation error");
        }

        var first = errors[0];
        var statusCode = first.Type switch
        {
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };

        var extensions = new Dictionary<string, object?>
        {
            ["errors"] = errors.Select(error => new { error.Code, error.Description }).ToArray()
        };

        return Results.Problem(title: first.Description, statusCode: statusCode, extensions: extensions);
    }
}
