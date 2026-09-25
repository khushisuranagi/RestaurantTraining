using RestaurantTraining.Web.Models;

namespace RestaurantTraining.Web.Services.PeopleManagement;

public interface IPeopleService
{
    Task<List<PeopleRoleGroup>> GetPeopleAsync();

    Task<LearnerProfileModel?> GetLearnerProfileAsync(int userId);

    Task<SetLearnerActiveStatusResponse?> SetLearnerActiveStatusAsync(int userId, bool isActive);
}