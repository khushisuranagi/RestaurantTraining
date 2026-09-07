using System.Net.Http.Json;
using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.PeopleManagement;

public class PeopleService : IPeopleService
{
    private readonly HttpClient _http;
    public PeopleService(HttpClient http) => _http = http;

    public async Task<List<PeopleRoleGroup>> GetPeopleAsync()
    {
        return await _http.GetFromJsonAsync<List<PeopleRoleGroup>>(
            "/api/content-creator/people") ?? [];
    }
}
