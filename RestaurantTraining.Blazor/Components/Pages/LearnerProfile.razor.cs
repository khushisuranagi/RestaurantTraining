using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RestaurantTraining.Blazor.Models;
using RestaurantTraining.Blazor.Services;
using RestaurantTraining.Blazor.Services.PeopleManagement;

namespace RestaurantTraining.Blazor.Components.Pages;

public partial class LearnerProfile
{
    [Parameter] public int UserId { get; set; }

   

    private LearnerProfileModel? profile;
    private bool isLoading = true;
    private bool isSavingStatus;
    private string statusMessage = string.Empty;
    private bool isStatusSuccess;

    protected override async Task OnInitializedAsync()
    {
        if (!AuthState.IsLoggedIn || AuthState.Role != "Content Creator")
        {
            Navigation.NavigateTo("/");
            return;
        }

        try
        {
            profile = await PeopleService.GetLearnerProfileAsync(UserId);
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task ToggleStatus(ChangeEventArgs e)
    {
        if (profile is null) return;

        var newValue = (bool)(e.Value ?? false);
        statusMessage = string.Empty;
        isSavingStatus = true;

        try
        {
            var result = await PeopleService.SetLearnerActiveStatusAsync(profile.UserId, newValue);

            if (result is null || !result.Success)
            {
                isStatusSuccess = false;
                statusMessage = result?.Message ?? "Could not update the account status. Please try again.";
                return;
            }

            profile.IsActive = result.IsActive;
            isStatusSuccess = true;
            statusMessage = result.Message;
        }
        catch
        {
            isStatusSuccess = false;
            statusMessage = "Could not reach the server. Please try again.";
        }
        finally
        {
            isSavingStatus = false;
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