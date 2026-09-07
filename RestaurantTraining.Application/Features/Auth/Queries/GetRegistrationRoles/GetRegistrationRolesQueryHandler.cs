using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.Auth.Queries.GetRegistrationRoles
{
    public class GetRegistrationRolesQueryHandler : IRequestHandler<GetRegistrationRolesQuery, List<RegistrationRoleDto>>
    {
        private readonly IAuthRepository _authRepository;

        public GetRegistrationRolesQueryHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<List<RegistrationRoleDto>> Handle(GetRegistrationRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _authRepository.GetSelfRegistrationRolesAsync(cancellationToken);
            return roles.Select(x => new RegistrationRoleDto
            {
                RoleId = x.RoleId,
                RoleName = x.RoleName
            }).ToList();
        }
    }
}
