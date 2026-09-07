using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.ProfileManagement;

public interface IProfileService
{
    Task<ProfileModel?> GetProfileAsync();
}

