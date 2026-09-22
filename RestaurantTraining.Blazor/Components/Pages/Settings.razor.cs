using System.Net.Http;
using Microsoft.AspNetCore.Components;
using RestaurantTraining.Blazor.Models;
using RestaurantTraining.Blazor.Services;
using RestaurantTraining.Blazor.Services.SettingsManagement;

namespace RestaurantTraining.Blazor.Components.Pages;

public partial class Settings
{
    [Inject]
    private ISettingsService SettingsService { get; set; } = default!;

    [Inject]
    private AuthState AuthState { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private HttpClient Http { get; set; } = default!;

    private string currentPassword = string.Empty;
    private string newPassword = string.Empty;
    private string confirmNewPassword = string.Empty;
    private string message = string.Empty;
    private bool isSuccess;
    private bool isSaving;

    // ADD THESE NEW FIELDS
    private string emailCurrentPassword = string.Empty;
    private string newEmail = string.Empty;
    private string emailMessage = string.Empty;
    private bool isEmailSuccess;
    private bool isSavingEmail;

    // ---- Delete account ----
    private string deletePassword = string.Empty;
    private string deleteMessage = string.Empty;
    private bool isDeleteSuccess;
    private bool isDeleting;
    private bool showDeleteConfirm;


    

    protected override async Task OnInitializedAsync()
    {
        if (!AuthState.IsLoggedIn) { Navigation.NavigateTo("/login"); }
        await Task.CompletedTask;
    }

    




    private async Task HandleChangePassword()
    {
        message = string.Empty;
        isSuccess = false;

        if (string.IsNullOrWhiteSpace(currentPassword) ||
            string.IsNullOrWhiteSpace(newPassword) ||
            string.IsNullOrWhiteSpace(confirmNewPassword))
        {
            message = "Please fill in all fields.";
            return;
        }

        isSaving = true;

        try
        {
            var request = new ChangePasswordRequest
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword,
                ConfirmNewPassword = confirmNewPassword
            };

            var result = await SettingsService.ChangePasswordAsync(request);

            message = result.Message;
            isSuccess = result.Success;

            if (isSuccess)
            {
                currentPassword = string.Empty;
                newPassword = string.Empty;
                confirmNewPassword = string.Empty;
            }
        }
        catch
        {
            message = "Could not reach the server. Please try again.";
        }
        finally
        {
            isSaving = false;
        }
    }

    // ADD THIS NEW METHOD
    private async Task HandleUpdateEmail()
    {
        emailMessage = string.Empty;
        isEmailSuccess = false;

        if (string.IsNullOrWhiteSpace(emailCurrentPassword) || string.IsNullOrWhiteSpace(newEmail))
        {
            emailMessage = "Please fill in both fields.";
            return;
        }

        isSavingEmail = true;

        try
        {
            var request = new UpdateEmailRequest
            {
                CurrentPassword = emailCurrentPassword,
                NewEmail = newEmail
            };

            var result = await SettingsService.UpdateEmailAsync(request);

            emailMessage = result.Message;
            isEmailSuccess = result.Success;

            if (isEmailSuccess)
            {
                emailCurrentPassword = string.Empty;
                newEmail = string.Empty;
            }
        }
        catch
        {
            emailMessage = "Could not reach the server. Please try again.";
        }
        finally
        {
            isSavingEmail = false;
        }
    }


    //active inactive


    // Step 1: the user clicked "Delete my account" — ask them to confirm.
    private void StartDeleteConfirm()
    {
        deleteMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(deletePassword))
        {
            isDeleteSuccess = false;
            deleteMessage = "Please enter your current password.";
            return;
        }

        showDeleteConfirm = true;
    }

    private void CancelDelete()
    {
        showDeleteConfirm = false;
        deleteMessage = string.Empty;
    }

    // Step 2: they confirmed — actually delete the account.
    private async Task HandleDeleteAccount()
    {
        deleteMessage = string.Empty;
        isDeleteSuccess = false;
        isDeleting = true;

        try
        {
            var result = await SettingsService.DeleteAccountAsync(new DeleteAccountRequest
            {
                CurrentPassword = deletePassword
            });

            if (result.Success)
            {
                // Clear the session (same as logging out) and go to login.
                AuthState.Token = string.Empty;
                AuthState.FullName = string.Empty;
                AuthState.Role = string.Empty;
                Http.DefaultRequestHeaders.Authorization = null;

                Navigation.NavigateTo("/login");
            }
            else
            {
                deleteMessage = result.Message;
                showDeleteConfirm = false;
            }
        }
        catch
        {
            deleteMessage = "Could not reach the server. Please try again.";
            showDeleteConfirm = false;
        }
        finally
        {
            isDeleting = false;
        }
    }

}