using RestaurantTraining.Web.Models;
namespace RestaurantTraining.Web.Services.DashboardManagement;


public interface IDashboardService
{
    Task<DashboardData> GetDashboardDataAsync();
    Task<ContentCreatorDashboardModel?> GetContentCreatorDashboardAsync();
}

