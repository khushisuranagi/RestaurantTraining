

namespace RestaurantTraining.Web.Services.ModuleManagement;

public interface IModuleService
{
    // Modules
    Task<List<ModuleSummary>> GetModulesAsync();

    Task<CreateModuleResult> CreateModuleAsync(
        string moduleName,
        string description,
        string? coverImageData = null,
    string? coverImageContentType = null);

    Task<ApiResponse> UpdateModuleAsync(
        int moduleId,
        string moduleName,
        string description,
        bool isActive,
        string? coverImageData = null,
    string? coverImageContentType = null);

    Task<ApiResponse> DeleteModuleAsync(int moduleId, bool confirmCascade = false);


    // Lessons
    Task<List<LessonSummary>> GetLessonsAsync();

    Task<CreateLessonResult> CreateLessonAsync(
        int moduleId,
        string lessonTitle,
        string description,
        int sortOrder);

    Task<UpdateLessonResult> UpdateLessonAsync(
        int lessonId,
        int moduleId,
        string lessonTitle,
        string description,
        int sortOrder,
        bool isActive);

    Task<ApiResponse> DeleteLessonAsync(
        int lessonId);


    // Resources
    Task<List<LessonResourceSummary>> GetResourcesAsync();

    Task<ApiResponse> CreateResourceAsync(
        int lessonId,
        string resourceType,
        string resourceUrl,
        string contentText,
        int sortOrder);

    Task<ApiResponse> UpdateResourceAsync(
        int resourceId,
        int lessonId,
        string resourceType,
        string resourceUrl,
        string contentText,
        int sortOrder,
        bool isActive);

    Task<ApiResponse> DeleteResourceAsync(
        int resourceId);

    Task<List<RoleOption>> GetRolesAsync();

    Task<List<ModuleAssignment>> GetAssignedRolesAsync(int moduleId);

    Task<ApiResponse> AssignModuleToRoleAsync(
        int moduleId,
        int roleId);

    Task<ApiResponse> UnassignModuleFromRoleAsync(
        int moduleId,
        int roleId);

}