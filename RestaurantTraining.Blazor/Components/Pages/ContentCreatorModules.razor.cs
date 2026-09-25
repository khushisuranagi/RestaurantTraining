using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using RestaurantTraining.Web.Services;
using RestaurantTraining.Web.Services.ModuleManagement;

namespace RestaurantTraining.Web.Components.Pages;

public partial class ContentCreatorModules
{
    [Inject]
    private AuthState AuthState { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private IModuleService ModuleService { get; set; } = default!;
    [Inject]
    private RestaurantTraining.Web.Services.LessonManagement.ILessonManagementService LessonManagementService { get; set; } = default!;
    private enum ResourceType
    {
        Video,
        Image,
        Article,
        Document
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
    private IBrowserFile? selectedModuleImageFile;
    private bool moduleHasExistingImage;




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

    private class LessonResourceRow
    {
        public int ResourceId { get; set; }   // 0 = new, otherwise an existing resource being edited
        public ResourceType Type { get; set; } = ResourceType.Video;
        public string Url { get; set; } = string.Empty;
        public string ContentText { get; set; } = string.Empty;
        public IBrowserFile? File { get; set; }
        public string? FileName { get; set; }
        public bool HasExistingFile { get; set; }   // true if an Image/PDF was already uploaded for this row
    }

    private List<int> originalResourceIds = [];   // resource ids that existed when the modal opened
    private List<LessonResourceRow> lessonResourceRows = [];
    private string formMessage = string.Empty;


    private void AddLessonResourceRow()
    {
        lessonResourceRows.Add(new LessonResourceRow());
    }

    private void RemoveLessonResourceRow(LessonResourceRow row)
    {
        lessonResourceRows.Remove(row);
    }

    private void HandleLessonResourceFileSelected(
        InputFileChangeEventArgs e, LessonResourceRow row)
    {
        row.File = e.File;
        row.FileName = e.File.Name;
        row.HasExistingFile = false; // a fresh file was chosen, so it's no longer "the old one"
    }
    
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
        selectedModuleImageFile = null;          // NEW
        moduleHasExistingImage = false;
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
        selectedModuleImageFile = null;
        moduleHasExistingImage = !string.IsNullOrEmpty(module.CoverImageData);

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
            string? imageData = null;
            string? imageContentType = null;
            if (selectedModuleImageFile != null)
            {
                using var stream = selectedModuleImageFile.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                imageData = Convert.ToBase64String(memoryStream.ToArray());
                imageContentType = selectedModuleImageFile.ContentType;
            }
            if (editingModule == null)
            {
                var result = await ModuleService.CreateModuleAsync(
    moduleName,
    moduleDescription, imageData,              // NEW
                imageContentType);

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
                        moduleIsActive,
                        imageData,
                        imageContentType);

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
    private bool cascadeConfirmNeeded;
    private int cascadeLessonCount;
    private void OpenDeleteModuleModal(ModuleSummary module)
    {
        moduleToDelete = module;
        showDeleteModuleModal = true;
    }


    private void CloseDeleteModuleModal()
    {
        moduleToDelete = null;
        showDeleteModuleModal = false;
        cascadeConfirmNeeded = false;
        cascadeLessonCount = 0;
    }


    private async Task DeleteModule() => await DeleteModuleInternal(false);          // first click
    private async Task ConfirmCascadeDelete() => await DeleteModuleInternal(true);   // "yes, delete lessons too"

    private async Task DeleteModuleInternal(bool confirmCascade)
    {
        if (moduleToDelete == null) return;

        errorMessage = string.Empty;

        try
        {
            var result = await ModuleService.DeleteModuleAsync(moduleToDelete.ModuleId, confirmCascade);

            if (result.RequiresConfirmation)      // module has lessons — escalate the modal
            {
                cascadeConfirmNeeded = true;
                cascadeLessonCount = result.LessonCount;
                StateHasChanged();                // make sure the warning re-renders
                return;                           // keep modal open, show the warning
            }

            if (!result.Success)
            {
                errorMessage = result.Message;
                StateHasChanged();
                return;
            }

            CloseDeleteModuleModal();
            await LoadModules();
        }
        catch (Exception ex)
        {
            // Surface the real reason instead of hiding it.
            errorMessage = $"Something went wrong while deleting the module: {ex.Message}";
        }

        StateHasChanged();
    }

    private void HandleModuleImageSelected(InputFileChangeEventArgs e)
    {
        selectedModuleImageFile = e.File;
        moduleHasExistingImage = false;
    }

    // LESSON CREATE / EDIT


    private void OpenCreateLessonModal()
    {
        editingLesson = null;

        lessonTitle = string.Empty;
        lessonDescription = string.Empty;
        lessonSortOrder = moduleLessons.Count + 1;
        lessonIsActive = true;
        resourceType = ResourceType.Video;
        resourceUrl = string.Empty;
        resourceContent = string.Empty;
        selectedResourceFile = null;

        formMessage = string.Empty;

        showLessonModal = true;
        lessonResourceRows = [new LessonResourceRow()];
        originalResourceIds = [];

    }


    private async Task OpenEditLessonModal(LessonSummary lesson)
    {
        editingLesson = lesson;

        lessonTitle = lesson.LessonTitle;
        lessonDescription = lesson.Description;
        lessonSortOrder = lesson.SortOrder;
        lessonIsActive = lesson.IsActive;

        var allResources = await ModuleService.GetResourcesAsync();

        var existing = allResources
            .Where(x => x.LessonId == lesson.LessonId)
            .OrderBy(x => x.SortOrder)
            .ToList();

        lessonResourceRows = existing.Count > 0
            ? existing.Select(r => new LessonResourceRow
            {
                ResourceId = r.ResourceId,
                Type = Enum.TryParse<ResourceType>(r.ResourceType.ToString(), true, out var t)
                    ? t : ResourceType.Video,
                Url = r.ResourceUrl,
                ContentText = r.ContentText,
                HasExistingFile = r.ResourceType is RestaurantTraining.Domain.Enums.ResourceType.Document
    or RestaurantTraining.Domain.Enums.ResourceType.Image   // <-- HERE, replacing the old FileName check
            }).ToList()
            : [new LessonResourceRow()];

        originalResourceIds = existing.Select(r => r.ResourceId).ToList();


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

                // If the creator filled in a resource, attach it to the new lesson.
                await SaveLessonResourcesAsync(result.LessonId);
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
                await SaveLessonResourcesAsync(editingLesson.LessonId);   // NEW — save resources on edit too
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

    // Saves one optional resource for a freshly-created lesson.
    // If nothing was entered, it quietly does nothing.
    // Saves every filled-in resource row for a freshly-created lesson.
    // Empty rows (nothing entered) are skipped.
    private async Task SaveLessonResourcesAsync(int lessonId)
    {
        foreach (var row in lessonResourceRows)
        {
            var isFileType =
                row.Type == ResourceType.Image ||
                row.Type == ResourceType.Document;

            var hasUrl = !string.IsNullOrWhiteSpace(row.Url);
            var hasFile = row.File != null;
            var isNewRow = row.ResourceId == 0;

            if (isNewRow)
            {
                // Skip rows the user left empty.
                if (isFileType && !hasFile) continue;
                if (!isFileType && !hasUrl) continue;
            }
            string? fileData = null;
            string? fileName = null;
            string? contentType = null;

            if (isFileType && row.File != null)
            {
                using var stream =row.File.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                fileData = Convert.ToBase64String(memoryStream.ToArray());
                fileName = row.File.Name;
                contentType = row.File.ContentType;
            }

            var request = new RestaurantTraining.Web.Models.SaveLessonResourceRequest
            {
                ResourceId = row.ResourceId,
                LessonId = lessonId,
                ResourceType = Enum.Parse<RestaurantTraining.Domain.Enums.ResourceType>(
                    row.Type.ToString()),
                ResourceUrl = row.Url,
                FileData = fileData,
                FileName = fileName,
                ContentType = contentType,
                ContentText = row.ContentText,
                SortOrder = lessonResourceRows.IndexOf(row) + 1,
                IsActive = true
            };

            await LessonManagementService.SaveResourceAsync(request);
        }
    // Delete resources that existed before but are no longer in the list.
    var currentExistingIds = lessonResourceRows
        .Where(r => r.ResourceId != 0)
        .Select(r => r.ResourceId)
        .ToList();

    var removedIds = originalResourceIds
        .Where(id => !currentExistingIds.Contains(id))
        .ToList();

    foreach (var id in removedIds)
    {
        await ModuleService.DeleteResourceAsync(id);
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
        resource.ResourceType.ToString(),
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