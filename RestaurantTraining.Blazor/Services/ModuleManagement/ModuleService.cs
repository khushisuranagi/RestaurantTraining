using System.Net.Http.Json;

namespace RestaurantTraining.Blazor.Services.ModuleManagement;

public class ModuleService : IModuleService
{
    private readonly HttpClient _http;

    public ModuleService(HttpClient http)
    {
        _http = http;
    }


    
    // MODULES
   

    public async Task<List<ModuleSummary>> GetModulesAsync()
    {
        return await _http.GetFromJsonAsync<List<ModuleSummary>>(
            "/api/Modules") ?? [];
    }


    public async Task<CreateModuleResult> CreateModuleAsync(
        string moduleName,
        string description)
    {
        var command = new
        {
            ModuleName = moduleName,
            Description = description
        };

        var response = await _http.PostAsJsonAsync(
            "/api/Modules",
            command);

        if (!response.IsSuccessStatusCode)
        {
            return new CreateModuleResult
            {
                Success = false,
                Message = "Could not create the module."
            };
        }

        return await response.Content.ReadFromJsonAsync<CreateModuleResult>()
               ?? new CreateModuleResult
               {
                   Success = false,
                   Message = "Could not read the create module response."
               };
    }


    public async Task<ApiResponse> UpdateModuleAsync(
        int moduleId,
        string moduleName,
        string description,
        bool isActive)
    {
        var command = new
        {
            ModuleId = moduleId,
            ModuleName = moduleName,
            Description = description,
            IsActive = isActive
        };

        var response = await _http.PutAsJsonAsync(
            $"/api/Modules/{moduleId}",
            command);

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse
            {
                Success = false,
                Message = "Could not update the module."
            };
        }

        return await response.Content.ReadFromJsonAsync<ApiResponse>()
               ?? new ApiResponse
               {
                   Success = true,
                   Message = "Module updated successfully."
               };
    }


    public async Task<ApiResponse> DeleteModuleAsync(int moduleId)
    {
        var response = await _http.DeleteAsync(
            $"/api/Modules/{moduleId}");

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse
            {
                Success = false,
                Message = "Could not delete the module."
            };
        }

        return await ReadApiResponseAsync(
            response,
            "Module deleted successfully.");
    }


    
    // LESSONS
   

    public async Task<List<LessonSummary>> GetLessonsAsync()
    {
        return await _http.GetFromJsonAsync<List<LessonSummary>>(
            "/api/Lessons") ?? [];
    }


    public async Task<CreateLessonResult> CreateLessonAsync(
        int moduleId,
        string lessonTitle,
        string description,
        int sortOrder)
    {
        var command = new
        {
            ModuleId = moduleId,
            LessonTitle = lessonTitle,
            Description = description,
            SortOrder = sortOrder
        };

        var response = await _http.PostAsJsonAsync(
            "/api/Lessons",
            command);

        if (!response.IsSuccessStatusCode)
        {
            return new CreateLessonResult
            {
                Success = false,
                Message = "Could not create the lesson."
            };
        }

        return await response.Content.ReadFromJsonAsync<CreateLessonResult>()
               ?? new CreateLessonResult
               {
                   Success = false,
                   Message = "Could not read the create lesson response."
               };
    }


    public async Task<UpdateLessonResult> UpdateLessonAsync(
        int lessonId,
        int moduleId,
        string lessonTitle,
        string description,
        int sortOrder,
        bool isActive)
    {
        var command = new
        {
            LessonId = lessonId,
            ModuleId = moduleId,
            LessonTitle = lessonTitle,
            Description = description,
            SortOrder = sortOrder,
            IsActive = isActive
        };

        var response = await _http.PutAsJsonAsync(
            $"/api/Lessons/{lessonId}",
            command);

        if (!response.IsSuccessStatusCode)
        {
            return new UpdateLessonResult
            {
                Success = false,
                Message = "Could not update the lesson."
            };
        }

        return await response.Content.ReadFromJsonAsync<UpdateLessonResult>()
               ?? new UpdateLessonResult
               {
                   Success = true,
                   Message = "Lesson updated successfully.",
                   LessonId = lessonId
               };
    }


    public async Task<ApiResponse> DeleteLessonAsync(int lessonId)
    {
        var response = await _http.DeleteAsync(
            $"/api/Lessons/{lessonId}");

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse
            {
                Success = false,
                Message = "Could not delete the lesson."
            };
        }

        return await ReadApiResponseAsync(
            response,
            "Lesson deleted successfully.");
    }


   
    // RESOURCES
   

    public async Task<List<LessonResourceSummary>> GetResourcesAsync()
    {
        return await _http.GetFromJsonAsync<List<LessonResourceSummary>>(
            "/api/LessonResources") ?? [];
    }


    public async Task<ApiResponse> CreateResourceAsync(
        int lessonId,
        string resourceType,
        string resourceUrl,
        string contentText,
        int sortOrder)
    {
        var command = new
        {
            LessonId = lessonId,
            ResourceType = resourceType,
            ResourceUrl = resourceUrl,
            ContentText = contentText,
            SortOrder = sortOrder
        };

        var response = await _http.PostAsJsonAsync(
            "/api/LessonResources",
            command);

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse
            {
                Success = false,
                Message = "Could not create the resource."
            };
        }

        return await ReadApiResponseAsync(
            response,
            "Resource created successfully.");
    }


    public async Task<ApiResponse> UpdateResourceAsync(
        int resourceId,
        int lessonId,
        string resourceType,
        string resourceUrl,
        string contentText,
        int sortOrder,
        bool isActive)
    {
        var command = new
        {
            ResourceId = resourceId,
            LessonId = lessonId,
            ResourceType = resourceType,
            ResourceUrl = resourceUrl,
            ContentText = contentText,
            SortOrder = sortOrder,
            IsActive = isActive
        };

        var response = await _http.PutAsJsonAsync(
            $"/api/LessonResources/{resourceId}",
            command);

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse
            {
                Success = false,
                Message = "Could not update the resource."
            };
        }

        return await ReadApiResponseAsync(
            response,
            "Resource updated successfully.");
    }


    public async Task<ApiResponse> DeleteResourceAsync(int resourceId)
    {
        var response = await _http.DeleteAsync(
            $"/api/LessonResources/{resourceId}");

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse
            {
                Success = false,
                Message = "Could not delete the resource."
            };
        }

        return await ReadApiResponseAsync(
            response,
            "Resource deleted successfully.");
    }


    public async Task<List<RoleOption>> GetRolesAsync()
    {
        return await _http.GetFromJsonAsync<List<RoleOption>>(
            "/api/Auth/registration-roles") ?? [];
    }

    public async Task<List<ModuleAssignment>> GetAssignedRolesAsync(
        int moduleId)
    {
        return await _http.GetFromJsonAsync<List<ModuleAssignment>>(
            $"/api/RoleModules/module/{moduleId}") ?? [];
    }

    public async Task<ApiResponse> AssignModuleToRoleAsync(
        int moduleId,
        int roleId)
    {
        var response = await _http.PostAsJsonAsync(
            "/api/RoleModules",
            new
            {
                ModuleId = moduleId,
                RoleId = roleId
            });

        return await ReadApiResponseAsync(
            response,
            "Role assigned successfully.");
    }

    public async Task<ApiResponse> UnassignModuleFromRoleAsync(
        int moduleId,
        int roleId)
    {
        var response = await _http.DeleteAsync(
            $"/api/RoleModules/role/{roleId}/module/{moduleId}");

        return await ReadApiResponseAsync(
            response,
            "Role unassigned successfully.");
    }

   
    // HELPER

    private static async Task<ApiResponse> ReadApiResponseAsync(
        HttpResponseMessage response,
        string defaultMessage)
    {
        try
        {
            var result =
                await response.Content.ReadFromJsonAsync<ApiResponse>();

            return result ?? new ApiResponse   //if no result return api response
            {
                Success = true,
                Message = defaultMessage
            };
        }
        catch
        {
            return new ApiResponse
            {
                Success = true,
                Message = defaultMessage
            };
        }
    }
}


// SERVICE MODELS


public class ModuleSummary
{
    public int ModuleId { get; set; }

    public string ModuleName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}


public class LessonSummary
{
    public int LessonId { get; set; }

    public int ModuleId { get; set; }

    public string LessonTitle { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public string Description { get; set; } = string.Empty;
}


public class LessonResourceSummary
{
    public int ResourceId { get; set; }

    public int LessonId { get; set; }

    public string ResourceType { get; set; } = string.Empty;

    public string ResourceUrl { get; set; } = string.Empty;

    public string ContentText { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }
}


public class ApiResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;
}


public class CreateModuleResult : ApiResponse
{
    public int ModuleId { get; set; }
}


public class CreateLessonResult : ApiResponse
{
    public int LessonId { get; set; }
}


public class UpdateLessonResult : ApiResponse
{
    public int LessonId { get; set; }
}

public class RoleOption
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}

public class ModuleAssignment
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
}


