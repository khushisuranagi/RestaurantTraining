using System.Net.Http.Json;
using RestaurantTraining.Web.Models;

namespace RestaurantTraining.Web.Services.LeaderboardManagement;

public class LeaderboardService : ILeaderboardService
{
    private readonly HttpClient _http;

    public LeaderboardService(HttpClient http)
    {
        _http = http;
    }

    public async Task<LeaderboardModel?> GetLeaderboardAsync()
    {
        return await _http.GetFromJsonAsync<LeaderboardModel>("/api/leaderboard");
    }
}