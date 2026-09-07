using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using RestaurantTraining.Blazor.Services;
using RestaurantTraining.Blazor.Services.ModuleManagement;

namespace RestaurantTraining.Blazor.Components.Pages;

public partial class ContentCreatorModules
{
    [Inject]
    private AuthState AuthState { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private IModuleService ModuleService { get; set; } = default!;

    private enum ResourceType
    {
        Video,
        Image,
        Article,
        Pdf
    }


    
    // MODULES
    [Parameter]
    public int? ModuleId { get; set; }

    private List<ModuleSummary> modules = [];
    private ModuleSummary? selectedModule;

    private bool isLoading = true;

    private string searchText = string.Empty;
    private string statusFilter = "All";

    private string errorMessage = string.Empty;

    private bool showModuleModal;
    private bool isSaving;

    private ModuleSummary? editingModule;
    private ModuleSummary? moduleToDelete;

    private string moduleName = string.Empty;
    private string moduleDescription = string.Empty;
    private bool moduleIsActive = true;

    private bool showDeleteModuleModal;


    
    // STATUS DROPDOWN HELPERS
    
    
    // Status dropdowns bind to Active/Inactive
    

    private string ModuleStatus
    {
        get => moduleIsActive ? "Active" : "Inactive";
        set => moduleIsActive = value == "Active";
    }

    private string LessonStatus
    {
        get => lessonIsActive ? "Active" : "Inactive";
        set => lessonIsActive = value == "Active";
    }

    private string ResourceStatus
    {
        get => resourceIsActive ? "Active" : "Inactive";
        set => resourceIsActive = value == "Active";
    }


    // LESSONS


    private List<LessonSummary> lessons = [];
    private List<LessonSummary> moduleLessons = [];

    private LessonSummary? selectedLesson;
    private LessonSummary? editingLesson;
    private LessonSummary? lessonToDelete;

    private bool isLoadingLessons;
    private bool showLessonModal;
    private bool showDeleteLessonModal;

    private string lessonTitle = string.Empty;
    private string lessonDescription = string.Empty;
    private int lessonSortOrder = 1;
    private bool lessonIsActive = true;


  
    // RESOURCES
   

    private List<LessonResourceSummary> resources = [];
    private List<LessonResourceSummary> lessonResources = [];

    private LessonResourceSummary? editingResource;
    private LessonResourceSummary? resourceToDelete;

    private bool isLoadingResources;
    private bool showResourceModal;
    private bool showDeleteResourceModal;

    private ResourceType resourceType = ResourceType.Video;

    private string resourceUrl = string.Empty;
    private string resourceContent = string.Empty;
    private int resourceSortOrder = 1;
    private bool resourceIsActive = true;

    private IBrowserFile? selectedResourceFile;


    private string formMessage = string.Empty;


  
    // FILTERED MODULES
  

    private List<ModuleSummary> FilteredModules =>
        modules
            .Where(module =>
                string.IsNullOrWhiteSpace(searchText) ||
                module.ModuleName.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase) ||
                module.Description.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase))
            .Where(module =>
                statusFilter == "All" ||
                (statusFilter == "Active" && module.IsActive) ||
                (statusFilter == "Inactive" && !module.IsActive))
            .ToList();


    // INITIAL LOAD
    

    protected override async Task OnParametersSetAsync()
    {
        if (!AuthState.IsLoggedIn ||
            AuthState.Role != "Content Creator")
        {
            Navigation.NavigateTo("/");
            return;
        }

        availableRoles = await ModuleService.GetRolesAsync();

        await LoadModules();

        if (ModuleId.HasValue)
        {
            selectedModule = modules.FirstOrDefault(
                x => x.ModuleId == ModuleId.Value);

            if (selectedModule != null)
            {
                await LoadLessonsForModule();
            }
        }
        else
        {
            selectedModule = null;
            selectedLesson = null;
            moduleLessons.Clear();
            lessonResources.Clear();
        }
        if (ModuleId.HasValue)
        {
            selectedModule = modules.FirstOrDefault(
                x => x.ModuleId == ModuleId.Value);

            if (selectedModule != null)
            {
                await LoadLessonsForModule();
            }
        }
        else
        {
            selectedModule = null;
            selectedLesson = null;

            moduleLessons.Clear();
            lessonResources.Clear();
        }
    }


    private async Task LoadModules()
    {
        isLoading = true;
        errorMessage = string.Empty;

        try
        {
            modules = await ModuleService.GetModulesAsync();
        }
        catch
        {
            errorMessage =
                "We could not load the modules. Please make sure the API is running.";
        }
        finally
        {
            isLoading = false;
        }
    }


    // OPEN MODULE
  

    private void OpenModule(ModuleSummary module)
    {
        selectedModule = module;
        selectedLesson = null;

        Navigation.NavigateTo(
            $"/modules/{module.ModuleId}");
    }


  

    // LESSON LOADING
  

    private async Task LoadLessonsForModule()
    {
        if (selectedModule == null)
            return;

        isLoadingLessons = true;
        errorMessage = string.Empty;

        try
        {
            lessons = await ModuleService.GetLessonsAsync();

            moduleLessons = lessons
                .Where(x =>
                    x.ModuleId == selectedModule.ModuleId)
                .OrderBy(x => x.SortOrder)
                .ToList();
        }
        catch
        {
            errorMessage =
                "We could not load the lessons.";
        }
        finally
        {
            isLoadingLessons = false;
        }
    }


  
    // OPEN LESSON


    private void OpenLesson(LessonSummary lesson)
    {
        selectedLesson = lesson;

        Navigation.NavigateTo(
            $"/lessons/{lesson.LessonId}");
    }


    private async Task LoadResourcesForLesson()
    {
        if (selectedLesson == null)
            return;

        isLoadingResources = true;
        errorMessage = string.Empty;

        try
        {
            resources = await ModuleService.GetResourcesAsync();

            lessonResources = resources
                .Where(x =>
                    x.LessonId == selectedLesson.LessonId)
                .OrderBy(x => x.SortOrder)
                .ToList();
        }
        catch
        {
            errorMessage =
                "We could not load the lesson resources.";
        }
        finally
        {
            isLoadingResources = false;
        }
    }


   
    // NAVIGATION
    private void BackToModules()
    {
        selectedModule = null;
        selectedLesson = null;

        moduleLessons.Clear();
        lessonResources.Clear();

        Navigation.NavigateTo("/modules");
    }


    private void BackToLessons()
    {
        selectedLesson = null;
        lessonResources.Clear();

        if (selectedModule != null)
        {
            Navigation.NavigateTo(
                $"/modules/{selectedModule.ModuleId}");
        }
        else
        {
            Navigation.NavigateTo("/modules");
        }
    }


    // MODULE CREATE / EDIT
    
    private void OpenCreateModuleModal()
    {
        editingModule = null;

        moduleName = string.Empty;
        moduleDescription = string.Empty;
        moduleIsActive = true;

        formMessage = string.Empty;

        showModuleModal = true;
        selectedRoleIds = [];
    }


    private async Task OpenEditModuleModal(ModuleSummary module)
    {
        editingModule = module;

        moduleName = module.ModuleName;
        moduleDescription = module.Description;
        moduleIsActive = module.IsActive;

        formMessage = string.Empty;

        showModuleModal = true;
        var assignedRoles =
    await ModuleService.GetAssignedRolesAsync(module.ModuleId);

        selectedRoleIds = assignedRoles
            .Select(x => x.RoleId)
            .ToHashSet();
    }


    private void CloseModuleModal()
    {
        showModuleModal = false;
        editingModule = null;
        formMessage = string.Empty;
    }


    private async Task SaveModule()
    {
        if (string.IsNullOrWhiteSpace(moduleName))
        {
            formMessage = "Module name is required.";
            return;
        }

        isSaving = true;
        formMessage = string.Empty;

        try
        {
            if (editingModule == null)
            {
                var result = await ModuleService.CreateModuleAsync(
    moduleName,
    moduleDescription);

                if (!result.Success)
                {
                    formMessage = result.Message;
                    return;
                }

                await SaveRoleAssignmentsAsync(result.ModuleId);
            }
            else
            {
                var result =
                    await ModuleService.UpdateModuleAsync(
                        editingModule.ModuleId,
                        moduleName,
                        moduleDescription,
                        moduleIsActive);

                if (!result.Success)
                {
                    formMessage = result.Message;
                    return;
                }
                await SaveRoleAssignmentsAsync(editingModule.ModuleId);
            }

            CloseModuleModal();

            await LoadModules();
        }
        catch
        {
            formMessage =
                "Something went wrong while saving the module.";
        }
        finally
        {
            isSaving = false;
        }
    }


    
    // MODULE DELETE
   
    private void OpenDeleteModuleModal(ModuleSummary module)
    {
        moduleToDelete = module;
        showDeleteModuleModal = true;
    }


    private void CloseDeleteModuleModal()
    {
        moduleToDelete = null;
        showDeleteModuleModal = false;
    }


    private async Task DeleteModule()
    {
        if (moduleToDelete == null)
            return;

        try
        {
            var result =
                await ModuleService.DeleteModuleAsync(
                    moduleToDelete.ModuleId);

            if (!result.Success)
            {
                errorMessage = result.Message;
                return;
            }

            CloseDeleteModuleModal();

            await LoadModules();
        }
        catch
        {
            errorMessage =
                "Something went wrong while deleting the module.";
        }
    }


    // LESSON CREATE / EDIT
   

    private void OpenCreateLessonModal()
    {
        editingLesson = null;

        lessonTitle = string.Empty;
        lessonDescription = string.Empty;
        lessonSortOrder = moduleLessons.Count + 1;
        lessonIsActive = true;

        formMessage = string.Empty;

        showLessonModal = true;
    }


    private void OpenEditLessonModal(LessonSummary lesson)
    {
        editingLesson = lesson;

        lessonTitle = lesson.LessonTitle;
        lessonDescription = lesson.Description;
        lessonSortOrder = lesson.SortOrder;
        lessonIsActive = lesson.IsActive;

        formMessage = string.Empty;

        showLessonModal = true;
    }


    private void CloseLessonModal()
    {
        showLessonModal = false;
        editingLesson = null;
        formMessage = string.Empty;
    }


    private async Task SaveLesson()
    {
        if (string.IsNullOrWhiteSpace(lessonTitle))
        {
            formMessage = "Lesson title is required.";
            return;
        }

        if (selectedModule == null)
            return;

        isSaving = true;
        formMessage = string.Empty;

        try
        {
            if (editingLesson == null)
            {
                var result =
                    await ModuleService.CreateLessonAsync(
                        selectedModule.ModuleId,
                        lessonTitle,
                        lessonDescription,
                        lessonSortOrder);

                if (!result.Success)
                {
                    formMessage = result.Message;
                    return;
                }
            }
            else
            {
                var result =
                    await ModuleService.UpdateLessonAsync(
                        editingLesson.LessonId,
                        selectedModule.ModuleId,
                        lessonTitle,
                        lessonDescription,
                        lessonSortOrder,
                        lessonIsActive);

                if (!result.Success)
                {
                    formMessage = result.Message;
                    return;
                }
            }

            CloseLessonModal();

            await LoadLessonsForModule();
        }
        catch
        {
            formMessage =
                "Something went wrong while saving the lesson.";
        }
        finally
        {
            isSaving = false;
        }
    }

    
    // LESSON DELETE


    private void OpenDeleteLessonModal(LessonSummary lesson)
    {
        lessonToDelete = lesson;
        showDeleteLessonModal = true;
    }


    private void CloseDeleteLessonModal()
    {
        lessonToDelete = null;
        showDeleteLessonModal = false;
    }


    private async Task DeleteLesson()
    {
        if (lessonToDelete == null)
            return;

        try
        {
            var result =
                await ModuleService.DeleteLessonAsync(
                    lessonToDelete.LessonId);

            if (!result.Success)
            {
                errorMessage = result.Message;
                return;
            }

            CloseDeleteLessonModal();

            await LoadLessonsForModule();
        }
        catch
        {
            errorMessage =
                "Something went wrong while deleting the lesson.";
        }
    }


    
    // RESOURCE CREATE / EDIT
  

    private void OpenCreateResourceModal()
    {
        editingResource = null;

        resourceType = ResourceType.Video;
        resourceUrl = string.Empty;
        resourceContent = string.Empty;
        resourceSortOrder = lessonResources.Count + 1;
        resourceIsActive = true;

        formMessage = string.Empty;

        showResourceModal = true;
        selectedResourceFile = null;
    }


    private void OpenEditResourceModal(
        LessonResourceSummary resource)
    {
        editingResource = resource;

        resourceType = Enum.TryParse<ResourceType>(
            resource.ResourceType,
            true,
            out var parsedType)
            ? parsedType
            : ResourceType.Video;

        resourceUrl = resource.ResourceUrl;
        resourceContent = resource.ContentText;
        resourceSortOrder = resource.SortOrder;
        resourceIsActive = resource.IsActive;

        formMessage = string.Empty;

        showResourceModal = true;
    }


    private void CloseResourceModal()
    {
        showResourceModal = false;
        editingResource = null;
        selectedResourceFile = null;
        formMessage = string.Empty;
    }


    private async Task SaveResource()
    {
        if (selectedLesson == null)
            return;

        isSaving = true;
        formMessage = string.Empty;

        try
        {
            var resourceTypeValue =
                resourceType.ToString();

            if (editingResource == null)
            {
                var result =
                    await ModuleService.CreateResourceAsync(
                        selectedLesson.LessonId,
                        resourceTypeValue,
                        resourceUrl,
                        resourceContent,
                        resourceSortOrder);

                if (!result.Success)
                {
                    formMessage = result.Message;
                    return;
                }
            }
            else
            {
                var result =
                    await ModuleService.UpdateResourceAsync(
                        editingResource.ResourceId,
                        selectedLesson.LessonId,
                        resourceTypeValue,
                        resourceUrl,
                        resourceContent,
                        resourceSortOrder,
                        resourceIsActive);

                if (!result.Success)
                {
                    formMessage = result.Message;
                    return;
                }
            }

            CloseResourceModal();

            await LoadResourcesForLesson();
        }
        catch
        {
            formMessage =
                "Something went wrong while saving the resource.";
        }
        finally
        {
            isSaving = false;
        }
    }


    
    // RESOURCE DELETE


    private void OpenDeleteResourceModal(
        LessonResourceSummary resource)
    {
        resourceToDelete = resource;
        showDeleteResourceModal = true;
    }


    private void CloseDeleteResourceModal()
    {
        resourceToDelete = null;
        showDeleteResourceModal = false;
    }


    private async Task DeleteResource()
    {
        if (resourceToDelete == null)
            return;

        try
        {
            var result =
                await ModuleService.DeleteResourceAsync(
                    resourceToDelete.ResourceId);

            if (!result.Success)
            {
                errorMessage = result.Message;
                return;
            }

            CloseDeleteResourceModal();

            await LoadResourcesForLesson();
        }
        catch
        {
            errorMessage =
                "Something went wrong while deleting the resource.";
        }
    }


    // RESOURCE FILE SELECTION
 

    private void HandleResourceFileSelected(
        InputFileChangeEventArgs e)
    {
        selectedResourceFile = e.File;
    }
    //role to module
    private List<RoleOption> availableRoles = [];

    private HashSet<int> selectedRoleIds = [];


    private void ToggleRole(int roleId)
    {
        if (!selectedRoleIds.Add(roleId))
        {
            selectedRoleIds.Remove(roleId);
        }
    }
    

    private async Task SaveRoleAssignmentsAsync(int moduleId)
    {
        var currentAssignments =
            await ModuleService.GetAssignedRolesAsync(moduleId);

        var currentlyAssignedIds = currentAssignments
            .Select(x => x.RoleId)
            .ToHashSet();

        // Add newly selected roles.
        foreach (var roleId in selectedRoleIds
            .Where(id => !currentlyAssignedIds.Contains(id)))
        {
            var result = await ModuleService
                .AssignModuleToRoleAsync(moduleId, roleId);

            if (!result.Success)
            {
                throw new Exception(result.Message);
            }
        }

        // Remove roles that are no longer selected.
        foreach (var roleId in currentlyAssignedIds
            .Where(id => !selectedRoleIds.Contains(id)))
        {
            var result = await ModuleService
                .UnassignModuleFromRoleAsync(moduleId, roleId);

            if (!result.Success)
            {
                throw new Exception(result.Message);
            }
        }
    }

}