using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.PeopleManagement;

public interface IPeopleService
{
    Task<List<PeopleRoleGroup>> GetPeopleAsync();

    Task<LearnerProfileModel?> GetLearnerProfileAsync(int userId);

    Task<SetLearnerActiveStatusResponse?> SetLearnerActiveStatusAsync(int userId, bool isActive);
}