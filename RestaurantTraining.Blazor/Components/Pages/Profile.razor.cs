using Microsoft.AspNetCore.Components;
using RestaurantTraining.Web.Models;
using RestaurantTraining.Web.Services;
using RestaurantTraining.Web.Services.ProfileManagement;
using RestaurantTraining.Web.Services.LeaderboardManagement;

namespace RestaurantTraining.Web.Components.Pages;

public partial class Profile
{
    [Inject]
    private IProfileService ProfileService { get; set; } = default!;

    [Inject]
    private AuthState AuthState { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private ILeaderboardService LeaderboardService { get; set; } = default!;

    private ProfileModel? profile;
    private bool isLoading = true;

    private bool isEditing;
    private bool isSaving;
    private string editFullName = string.Empty;
    private string editPhoneNumber = string.Empty;

    private string message = string.Empty;
    private bool isSuccess;

    private bool isOutstanding;   // top-3 on the leaderboard

    protected override async Task OnInitializedAsync()
    {
        if (!AuthState.IsLoggedIn) { Navigation.NavigateTo("/login"); return; }

        try
        {
            profile = await ProfileService.GetProfileAsync();

            // Show the "Outstanding Performance" badge if this learner is top 3.
            var leaderboard = await LeaderboardService.GetLeaderboardAsync();
            isOutstanding = leaderboard?.Me?.IsOutstanding ?? false;
        }
        finally
        {
            isLoading = false;
        }
    }

    private void StartEdit()
    {
        if (profile is null) return;
        editFullName = profile.FullName;
        editPhoneNumber = profile.PhoneNumber;
        message = string.Empty;
        isEditing = true;
    }

    private void CancelEdit()
    {
        isEditing = false;
        message = string.Empty;
    }

    private async Task SaveEdit()
    {
        if (profile is null) return;

        if (string.IsNullOrWhiteSpace(editFullName))
        {
            isSuccess = false;
            message = "Full name is required.";
            return;
        }

        isSaving = true;
        message = string.Empty;

        try
        {
            var result = await ProfileService.UpdateProfileAsync(new UpdateProfileRequest
            {
                FullName = editFullName.Trim(),
                PhoneNumber = editPhoneNumber?.Trim() ?? string.Empty
            });

            isSuccess = result.Success;
            message = result.Message;

            if (result.Success)
            {
                // Reflect the change on screen and in the top bar.
                profile.FullName = editFullName.Trim();
                profile.PhoneNumber = editPhoneNumber?.Trim() ?? string.Empty;
                AuthState.FullName = profile.FullName;
                isEditing = false;
            }
        }
        catch
        {
            isSuccess = false;
            message = "Something went wrong while saving your profile.";
        }
        finally
        {
            isSaving = false;
        }
    }

    private static string Initials(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "?";
        return string.Concat(name
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Take(2)
            .Select(w => char.ToUpperInvariant(w[0])));
    }
}