using ErrorOr;
using MediatR;
using StudyQuest.API.Services.Interfaces;

namespace StudyQuest.API.Features.Enrollments.RegisterDevice;

public record RegisterDeviceCommand(Guid StudentId, string Token, string Platform) : IRequest<ErrorOr<Unit>>;

internal sealed class RegisterDeviceCommandHandler : IRequestHandler<RegisterDeviceCommand, ErrorOr<Unit>>
{
    private readonly INotificationService _notificationService;

    public RegisterDeviceCommandHandler(INotificationService notificationService) => _notificationService = notificationService;

    public async Task<ErrorOr<Unit>> Handle(RegisterDeviceCommand request, CancellationToken ct)
    {
        await _notificationService.RegisterDeviceTokenAsync(request.StudentId, request.Token, request.Platform);
        return Unit.Value;
    }
}
