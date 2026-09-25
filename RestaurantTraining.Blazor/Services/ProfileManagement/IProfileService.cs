using RestaurantTraining.Web.Models;

namespace RestaurantTraining.Web.Services.ProfileManagement;

public interface IProfileService
{
    Task<ProfileModel?> GetProfileAsync();

    Task<ProfileUpdateResult> UpdateProfileAsync(UpdateProfileRequest request);
}

