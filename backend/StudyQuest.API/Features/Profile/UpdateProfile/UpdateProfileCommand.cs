using ErrorOr;
using MediatR;
using StudyQuest.API.Data;
using StudyQuest.API.Features.Auth.Common;

namespace StudyQuest.API.Features.Profile.UpdateProfile;

public record UpdateProfileCommand(
    Guid StudentId,
    string? FirstName,
    string? LastName,
    int? Grade,
    int? DailyGoalMinutes) : IRequest<ErrorOr<StudentResponse>>;

internal sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ErrorOr<StudentResponse>>
{
    private readonly AppDbContext _dbContext;

    public UpdateProfileCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<StudentResponse>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students.FindAsync([request.StudentId], cancellationToken);
        if (student is null)
        {
            return AuthErrors.StudentNotFound;
        }

        if (request.Grade.HasValue && request.Grade.Value is < 9 or > 12)
        {
            return Error.Validation(
                code: "Profile.InvalidGrade",
                description: "Grade must be between 9 and 12.");
        }

        if (request.DailyGoalMinutes.HasValue && request.DailyGoalMinutes <= 0)
        {
            return Error.Validation(
                code: "Profile.InvalidGoal",
                description: "Daily goal must be greater than zero minutes.");
        }

        if (request.FirstName is not null)
        {
            student.FirstName = request.FirstName.Trim();
        }

        if (request.LastName is not null)
        {
            student.LastName = request.LastName.Trim();
        }

        if (request.Grade.HasValue)
        {
            student.Grade = request.Grade.Value;
        }

        if (request.DailyGoalMinutes.HasValue)
        {
            student.DailyGoalMinutes = request.DailyGoalMinutes.Value;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return student.ToResponse();
    }
}
