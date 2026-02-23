using StudyQuest.API.DTOs.Enrollments;

namespace StudyQuest.API.Services.Interfaces;

public interface IEnrollmentService
{
    Task<List<EnrollmentDto>> GetEnrollmentsAsync(Guid studentId);
    Task<EnrollmentDto> EnrollAsync(Guid studentId, CreateEnrollmentDto dto);
    Task<bool> UnenrollAsync(Guid studentId, Guid enrollmentId);
}
