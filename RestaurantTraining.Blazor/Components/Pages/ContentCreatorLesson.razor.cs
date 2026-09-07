using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using RestaurantTraining.Blazor.Services;
using RestaurantTraining.Blazor.Models;
using RestaurantTraining.Blazor.Services.LessonManagement;
using RestaurantTraining.Domain.Enums;

namespace RestaurantTraining.Blazor.Components.Pages;

public partial class ContentCreatorLesson
{
    [Parameter]
    public int LessonId { get; set; }

    [Inject]
    public ILessonManagementService LessonManagementService { get; set; } = default!;

    [Inject]
    public IJSRuntime JS { get; set; } = default!;  //tool call js

    [Inject]
    public AuthState AuthState { get; set; } = default!;

    [Inject]
    public NavigationManager Navigation { get; set; } = default!;


    
    // LESSON

    private LessonSummary? lesson;

    private bool isLoading = true;


 
    // RESOURCE MODAL
    private bool showResourceModal;
    private bool isSaving;

    private string saveError = string.Empty;

    // 0 = adding a new resource
    // Any other value = editing an existing resource.
    private int editingResourceId = 0;

    private ResourceType resourceType = ResourceType.Video;

    private string resourceUrl = string.Empty;

    private string resourceContent = string.Empty;

    private int resourceSortOrder = 1;

    private bool resourceIsActive = true;

    private IBrowserFile? selectedFile;

    private List<LessonResourceSummary> resources = [];


    
    // INITIAL LOAD
   
    protected override async Task OnInitializedAsync()
    {
        if (!AuthState.IsLoggedIn ||
            AuthState.Role != "Content Creator")
        {
            Navigation.NavigateTo("/");
            return;
        }

        await LoadLesson();

        if (lesson != null)
        {
            await LoadResources();
        }
    }


   
    // LOAD LESSON
    private string GetFileDataUrl(
    LessonResourceSummary resource)
    {
        if (string.IsNullOrWhiteSpace(resource.FileData))
            return string.Empty;

        if (string.IsNullOrWhiteSpace(resource.ContentType))
            return string.Empty;

        return $"data:{resource.ContentType};base64,{resource.FileData}";   //builds data url and browser reads and displays
    }
    private async Task LoadLesson()
    {
        try
        {
            lesson =
                await LessonManagementService
                    .GetLessonByIdAsync(LessonId);   //ask service for lesson thru lessonid
        }
        catch
        {
            lesson = null;
        }
        finally
        {
            isLoading = false;
        }
    }


  
    // LOAD RESOURCES
  

    private async Task LoadResources()
    {
        try
        {
            resources =
                await LessonManagementService
                    .GetResourcesAsync();
        }
        catch
        {
            resources = [];
        }
    }


    // NAVIGATION
    private void BackToLessons()
    {
        if (lesson != null)
        {
            Navigation.NavigateTo(
                $"/modules/{lesson.ModuleId}");
        }
    }

    // CREATE RESOURCE
   
    private void OpenCreateResourceModal()
    {
        editingResourceId = 0;

        resourceType = ResourceType.Video;

        resourceUrl = string.Empty;

        resourceContent = string.Empty;

        resourceSortOrder = 1;

        resourceIsActive = true;

        selectedFile = null;

        saveError = string.Empty;

        showResourceModal = true;
    }


    // EDIT RESOURCE
   
    private void OpenEditResourceModal(
        LessonResourceSummary resource)
    {
        editingResourceId = resource.ResourceId;

        resourceType = resource.ResourceType;

        resourceUrl = resource.ResourceUrl;

        resourceContent = resource.ContentText;

        resourceSortOrder = resource.SortOrder;

        resourceIsActive = resource.IsActive;

       
        selectedFile = null;

        saveError = string.Empty;

        showResourceModal = true;
    }


    // DELETE RESOURCE


    private async Task DeleteResource(
        LessonResourceSummary resource)
    {
        var confirmed =
            await JS.InvokeAsync<bool>(
                "confirm",
                "Are you sure you want to delete this resource?");

        if (!confirmed)
            return;

        var success =
            await LessonManagementService
                .DeleteResourceAsync(
                    resource.ResourceId);

        if (success)
        {
            await LoadResources();
        }
        else
        {
            saveError =
                "Could not delete the resource.";
        }
    }


    // CHECK RESOURCE
   

    private bool LessonHasResource()
    {
        if (lesson == null)
            return false;

        return resources.Any(
            x => x.LessonId == lesson.LessonId);
    }


   
    // YOUTUBE EMBED
   

    private string? GetYouTubeEmbedUrl(
        string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;

        var videoId = string.Empty;

        if (url.Contains("youtu.be/"))
        {
            videoId =
                url.Split("youtu.be/")[1];
        }
        else if (url.Contains("watch?v="))
        {
            videoId =
                url.Split("watch?v=")[1];
        }
        else
        {
            return null;
        }

        videoId =
            videoId
                .Split('?')[0]
                .Split('&')[0];

        if (string.IsNullOrWhiteSpace(videoId))
            return null;

        return
            $"https://www.youtube.com/embed/{videoId}";
    }


   
    // CLOSE RESOURCE MODAL
    

    private void CloseResourceModal()
    {
        showResourceModal = false;

        selectedFile = null;

        saveError = string.Empty;
    }


    // FILE SELECTED
    

    private void HandleFileSelected(
        InputFileChangeEventArgs e)
    {
        selectedFile = e.File;
    }


    
    // SAVE RESOURCE
    

    private async Task SaveResource()
    {
        if (lesson == null)
            return;

        isSaving = true;
        saveError = string.Empty;

        try
        {
            string? fileData = null;
            string? fileName = null;
            string? contentType = null;

            var isFileType =
                resourceType == ResourceType.Image ||
                resourceType == ResourceType.Pdf;


 
            // CONVERT FILE TO BASE64
 

            if (isFileType)
            {
                if (selectedFile != null)
                {
                    // 10 MB maximum file size.
                    using var stream =
                        selectedFile.OpenReadStream(
                            maxAllowedSize: 10 * 1024 * 1024);

                    using var memoryStream =
                        new MemoryStream();

                    await stream.CopyToAsync(
                        memoryStream);

                    var fileBytes =
                        memoryStream.ToArray();  //raw file 

                    fileData =
                        Convert.ToBase64String(       //bytes to base64 text
                            fileBytes);

                    fileName =
                        selectedFile.Name;

                    contentType =
                        selectedFile.ContentType;
                }
                else if (editingResourceId == 0)
                {
                    saveError =
                        "Please select a file to upload.";

                    return;
                }
            }


 
            // BUILD REQUEST
 

            var request =
                new SaveLessonResourceRequest
                {
                    ResourceId =
                        editingResourceId,

                    LessonId =
                        lesson.LessonId,

                    ResourceType =
                        resourceType,

                    ResourceUrl =
                        resourceUrl,

                    FileData =
                        fileData,

                    FileName =
                        fileName,

                    ContentType =
                        contentType,

                    ContentText =
                        resourceContent,

                    SortOrder =
                        resourceSortOrder,

                    IsActive =
                        resourceIsActive
                };


 
            // SAVE THROUGH SERVICE
 

            var result =
                await LessonManagementService
                    .SaveResourceAsync(request);


            if (result.Success)
            {
                CloseResourceModal();

                await LoadResources();
            }
            else
            {
                saveError =
                    string.IsNullOrWhiteSpace(result.Message)
                        ? "Could not save the resource."
                        : result.Message;
            }
        }
        catch
        {
            saveError =
                "Something went wrong while saving the resource.";
        }
        finally
        {
            isSaving = false;
        }
    }
}