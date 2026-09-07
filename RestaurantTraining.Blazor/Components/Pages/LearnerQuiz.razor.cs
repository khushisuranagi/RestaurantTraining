using Microsoft.AspNetCore.Components;
using RestaurantTraining.Blazor.Models;
using RestaurantTraining.Blazor.Services;
using RestaurantTraining.Blazor.Services.LearnerQuizManagement;

namespace RestaurantTraining.Blazor.Components.Pages;

public partial class LearnerQuiz
{
    [Parameter]
    public int ModuleId { get; set; }

    [Inject]
    private ILearnerQuizService QuizService { get; set; } = default!;

    [Inject]
    private AuthState AuthState { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    private LearnerQuizModel? quiz;

    private QuizResultModel? result;

    private bool isLoading = true;

    private bool isSubmitting;

    private Dictionary<int, HashSet<int>> selected = new();

    private Dictionary<int, string> textAnswers = new();

    protected override async Task OnInitializedAsync()
    {
        if (!AuthState.IsLoggedIn || AuthState.Role == "Content Creator")
        {
            Navigation.NavigateTo("/login");
            return;
        }

        await Load();
    }

    private async Task Load()
    {
        isLoading = true;
        result = null;

        try
        {
            quiz = await QuizService.GetQuizAsync(ModuleId);

            selected = new();
            textAnswers = new();

            if (quiz is not null)
            {
                foreach (var q in quiz.Questions)
                {
                    selected[q.QuestionId] = new HashSet<int>();

                    textAnswers[q.QuestionId] = string.Empty;
                }
            }
        }
        finally
        {
            isLoading = false;
        }
    }

    private void ToggleOption(
        int questionId,
        int optionId,
        bool isChecked)
    {
        if (isChecked)
        {
            selected[questionId].Add(optionId);
        }
        else
        {
            selected[questionId].Remove(optionId);
        }
    }

    private void SelectSingle(
        int questionId,
        int optionId)
    {
        selected[questionId] = new HashSet<int>
        {
            optionId
        };
    }

    private async Task Submit()
    {
        if (quiz is null)
        {
            return;
        }

        isSubmitting = true;

        try
        {
            var request = new QuizSubmitRequest
            {
                Answers = quiz.Questions.Select(q => new QuizAnswerModel
                {
                    QuestionId = q.QuestionId,

                    SelectedOptionIds =
                        selected[q.QuestionId].ToList(),

                    TextAnswer =
                        textAnswers.TryGetValue(
                            q.QuestionId,
                            out var t)
                            ? t
                            : null

                }).ToList()
            };

            result =
                await QuizService.SubmitQuizAsync(
                    ModuleId,
                    request);
        }
        finally
        {
            isSubmitting = false;
        }
    }

    private async Task Retry()
    {
        await Load();
    }
}