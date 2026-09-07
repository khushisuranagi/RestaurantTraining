using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.ContentCreatorPeople.Queries.GetPeople
{
    public class GetPeopleQueryHandler
        : IRequestHandler<GetPeopleQuery, List<PeopleRoleGroupDto>>
    {
        private readonly IContentCreatorPeopleRepository _repository;

        public GetPeopleQueryHandler(
            IContentCreatorPeopleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PeopleRoleGroupDto>> Handle(
            GetPeopleQuery request,
            CancellationToken cancellationToken)
        {
            var roles = await _repository.GetRolesAsync(cancellationToken);
            var users = await _repository.GetUsersAsync(cancellationToken);
            var roleModules = await _repository.GetRoleModulesAsync(cancellationToken);

            return roles
                .Select(r => new PeopleRoleGroupDto
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    Users = users
                        .Where(u => u.RoleId == r.RoleId)
                        .Select(u => new PeopleUserDto
                        {
                            UserId = u.UserId,
                            FullName = u.FullName,
                            Email = u.Email
                        })
                        .ToList(),
                    Modules = roleModules
                        .Where(rm => rm.RoleId == r.RoleId)
                        .Select(rm => new PeopleModuleDto
                        {
                            ModuleId = rm.ModuleId,
                            ModuleName = rm.ModuleName
                        })
                        .ToList()
                })
                // Only show roles that actually have people or assigned modules.
                .Where(x => x.Users.Count > 0 || x.Modules.Count > 0)
                .OrderByDescending(x => x.Users.Count)
                .ThenBy(x => x.RoleName)
                .ToList();
        }
    }
}
