using Microsoft.AspNetCore.Components;
using RestaurantTraining.Blazor.Models;
using RestaurantTraining.Blazor.Services;
using RestaurantTraining.Blazor.Services.DashboardManagement;

namespace RestaurantTraining.Blazor.Components.Pages;

public partial class ContentCreatorDashboard
{
    [Inject] private IDashboardService DashboardService { get; set; } = default!;
    [Inject] private AuthState AuthState { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private ContentCreatorDashboardModel? data;
    private bool isLoading = true;
    private string errorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        if (!AuthState.IsLoggedIn || AuthState.Role != "Content Creator")
        {
            Navigation.NavigateTo("/");
            return;
        }

        try
        {
            data = await DashboardService.GetContentCreatorDashboardAsync();
            if (data is null)
                errorMessage = "We could not load your dashboard. Please confirm the API is running.";
        }
        catch
        {
            errorMessage = "We could not load your dashboard. Please confirm the API is running.";
        }
        finally
        {
            isLoading = false;
        }
    }

    // Total number of "needs attention" items across all categories.
    private int AttentionCount => data is null ? 0 :
        data.ModulesWithoutLessons.Count +
        data.ModulesWithoutQuiz.Count +
        data.ModulesNotAssigned.Count +
        data.LessonsWithoutResources.Count;

    private void OpenModule(int moduleId) => Navigation.NavigateTo($"/modules/{moduleId}");
    private void OpenQuizFor(int moduleId) => Navigation.NavigateTo($"/quiz/{moduleId}");
}
