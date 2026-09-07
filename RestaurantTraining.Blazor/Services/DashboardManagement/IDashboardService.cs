using RestaurantTraining.Blazor.Models;
namespace RestaurantTraining.Blazor.Services.DashboardManagement;


public interface IDashboardService
{
    Task<DashboardData> GetDashboardDataAsync();
    Task<ContentCreatorDashboardModel?> GetContentCreatorDashboardAsync();
}

