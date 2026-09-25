using RestaurantTraining.Web.Models;

namespace RestaurantTraining.Web.Services.LeaderboardManagement;

public interface ILeaderboardService
{
    Task<LeaderboardModel?> GetLeaderboardAsync();
}