using MediatR;

namespace RestaurantTraining.Application.Features.ContentCreatorPeople.Queries.GetPeople
{
    // Read request: "give me all users grouped by role, with each role's modules".
    public class GetPeopleQuery : IRequest<List<PeopleRoleGroupDto>>
    {
    }
}
