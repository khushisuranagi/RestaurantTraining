using MediatR;

namespace RestaurantTraining.Application.Features.Auth.Queries.GetRegistrationRoles
{
    public class GetRegistrationRolesQuery : IRequest<List<RegistrationRoleDto>>
    {
    }
}
