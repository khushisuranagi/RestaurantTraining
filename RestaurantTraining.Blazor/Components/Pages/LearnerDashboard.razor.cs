using Microsoft.AspNetCore.Components;
using RestaurantTraining.Blazor.Models;
using RestaurantTraining.Blazor.Services;
using RestaurantTraining.Blazor.Services.LearnerDashboardManagement;

namespace RestaurantTraining.Blazor.Components.Pages;

public partial class LearnerDashboard
{
    [Inject] private ILearnerDashboardService LearnerDashboardService { get; set; } = default!;
    [Inject] private AuthState AuthState { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;

    private LearnerDashboardModel? dashboard;
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        if (!AuthState.IsLoggedIn || AuthState.Role == "Content Creator")
        {
            Navigation.NavigateTo("/login");
            return;
        }

        try
        {
            dashboard = await LearnerDashboardService.GetDashboardAsync();
        }
        finally
        {
            isLoading = false;
        }
    }

    private void OpenModule(int moduleId) =>
        Navigation.NavigateTo($"/learning-modules/{moduleId}");

    private void ContinueLearning()
    {
       //goes to first oending module
        var next = dashboard?.PendingModules
            .FirstOrDefault(m => !m.IsCompleted);

        if (next != null)
            OpenModule(next.ModuleId);
        else
            Navigation.NavigateTo("/learning-modules");
    }
}