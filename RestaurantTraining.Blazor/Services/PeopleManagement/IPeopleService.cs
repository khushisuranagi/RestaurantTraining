using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.PeopleManagement;

public interface IPeopleService
{
    Task<List<PeopleRoleGroup>> GetPeopleAsync();
}
