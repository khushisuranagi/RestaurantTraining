using MediatR;

namespace RestaurantTraining.Application.Features.Auth.Commands.UpdateEmail
{
    public class UpdateEmailCommand : IRequest<UpdateEmailResponse>
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewEmail { get; set; } = string.Empty;
    }
}