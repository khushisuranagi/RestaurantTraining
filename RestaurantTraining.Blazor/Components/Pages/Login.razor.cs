using Microsoft.AspNetCore.Components;
using RestaurantTraining.Blazor.Models;
using RestaurantTraining.Blazor.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RestaurantTraining.Blazor.Components.Pages;

public partial class Login
{
    [Inject]
    private HttpClient Http { get; set; } = default!;

    [Inject]
    private AuthState AuthState { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    private string email = string.Empty;
    private string password = string.Empty;
    private string message = string.Empty;
    private bool isLoading = false;

    private async Task HandleLogin()
    {
        message = string.Empty;

      //no empty val
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            message = "Please enter both email and password.";
            return;
        }

        isLoading = true;

        try
        {
            // send  email and password to the api login endpoint
            var loginData = new
            {
                Email = email,
                Password = password
            };

            var response =
                await Http.PostAsJsonAsync(
                    "/api/Auth/login",
                    loginData);

            // reads the api's answer into  LoginResult class
            var result =
                await response.Content.ReadFromJsonAsync<LoginResult>();

            if (result == null || !result.Success)
            {
                // show the message the API gave us.
                message =
                    result?.Message ??
                    "Login failed. Please try again.";

                return;
            }

            // login works 
            AuthState.Token = result.Token;
            AuthState.FullName = result.FullName;
            AuthState.Role = result.Role;

            Http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    result.Token);

            // send the user to the right place based on their role.
            if (result.Role == "Content Creator")
            {
                Navigation.NavigateTo(
                    "/content-creator-dashboard");
            }
            else
            {
                Navigation.NavigateTo(
                    "/learner-dashboard");
            }
        }
        catch
        {
            // This usually means the API is not running.
            message =
                "Could not reach the server. Please make sure the API is running.";
        }
        finally
        {
            isLoading = false;
        }
    }
}