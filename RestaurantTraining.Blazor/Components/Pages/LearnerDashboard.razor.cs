using Microsoft.AspNetCore.Components;
using RestaurantTraining.Web.Models;
using RestaurantTraining.Web.Services;
using RestaurantTraining.Web.Services.LeaderboardManagement;
using RestaurantTraining.Web.Services.LearnerDashboardManagement;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace RestaurantTraining.Web.Components.Pages;

public partial class LearnerDashboard
{
    [Inject] private ILearnerDashboardService LearnerDashboardService { get; set; } = default!;
    [Inject] private AuthState AuthState { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;


 

    private bool isOutstanding;
    private int myRank;

    
    
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
            var lb = await LeaderboardService.GetLeaderboardAsync();
            if (lb?.Me is not null) { isOutstanding = lb.Me.IsOutstanding; myRank = lb.Me.Rank; }
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
        //goes to first pending module
        var next = dashboard?.PendingModules
            .FirstOrDefault(m => !m.IsCompleted);

        if (next != null)
            OpenModule(next.ModuleId);
        else
            Navigation.NavigateTo("/learning-modules");
    }
}