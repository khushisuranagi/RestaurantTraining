using RestaurantTraining.Blazor.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace RestaurantTraining.Blazor.Services.CertificateManagement;

public class CertificateService : ICertificateService
{
    private readonly HttpClient _http;

    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    public CertificateService(HttpClient http)
    {
        _http = http;
    }

 
    public async Task<CertificateSettingModel?> GetSettingAsync(int moduleId)
    {
        var response = await _http.GetAsync(
            $"/api/ModuleCertificateSettings/{moduleId}");

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();

        // Empty body or the literal "null" both mean "nothing configured".
        if (string.IsNullOrWhiteSpace(json) || json.Trim() == "null")
            return null;

        return JsonSerializer.Deserialize<CertificateSettingModel>(
            json, JsonOptions);
    }

    // Create or update 
    public async Task<CertificateSaveResult> SaveSettingAsync(
        SaveCertificateSettingRequest request)
    {
        var response = await _http.PutAsJsonAsync(
            $"/api/ModuleCertificateSettings/{request.ModuleId}",
            request);

        if (response.IsSuccessStatusCode)
        {
            return new CertificateSaveResult
            {
                Success = true,
                Message = "Certificate settings saved."
            };
        }

        try
        {
            var result = await response.Content
                .ReadFromJsonAsync<CertificateSaveResult>();

            return result ?? new CertificateSaveResult
            {
                Success = false,
                Message = "Could not save the certificate settings."
            };
        }
        catch
        {
            return new CertificateSaveResult
            {
                Success = false,
                Message = "Could not save the certificate settings."
            };
        }
    }
}