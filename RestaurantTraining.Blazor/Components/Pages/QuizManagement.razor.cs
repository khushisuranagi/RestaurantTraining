using Microsoft.AspNetCore.Components;
using RestaurantTraining.Blazor.Services;
using RestaurantTraining.Blazor.Services.ModuleManagement;
using RestaurantTraining.Blazor.Services.QuizManagement;

namespace RestaurantTraining.Blazor.Components.Pages;

public partial class QuizManagement
{
    [Inject] public IModuleService ModuleService { get; set; } = default!;
    [Inject] public IQuizService QuizService { get; set; } = default!;
    [Inject] public AuthState AuthState { get; set; } = default!;
    [Inject] public NavigationManager Navigation { get; set; } = default!;

    private List<ModuleSummary> modules = [];

    // moduleId -> number of quiz questions
    private Dictionary<int, int> questionCounts = new();

    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        if (!AuthState.IsLoggedIn || AuthState.Role != "Content Creator")
        {
            Navigation.NavigateTo("/");
            return;
        }

        try
        {
            modules = await ModuleService.GetModulesAsync();

            // One call returns all questions; group them by module to get counts.
            var questions = await QuizService.GetQuestionsAsync();

            questionCounts = questions
                .GroupBy(q => q.ModuleId)
                .ToDictionary(g => g.Key, g => g.Count());
        }
        catch
        {
            modules = [];
            questionCounts = new();
        }
        finally
        {
            isLoading = false;
        }
    }

    private int QuestionCount(int moduleId) =>
        questionCounts.TryGetValue(moduleId, out var count) ? count : 0;

    private void ManageQuiz(int moduleId) =>
        Navigation.NavigateTo($"/quiz/{moduleId}");
}
