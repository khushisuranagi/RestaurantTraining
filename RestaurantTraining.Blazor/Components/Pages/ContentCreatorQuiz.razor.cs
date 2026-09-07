using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RestaurantTraining.Blazor.Models;
using RestaurantTraining.Blazor.Services;
using RestaurantTraining.Blazor.Services.CertificateManagement;
using RestaurantTraining.Blazor.Services.QuizManagement;
using RestaurantTraining.Domain.Enums;

namespace RestaurantTraining.Blazor.Components.Pages;

public partial class ContentCreatorQuiz
{
    
    // PARAMETERS
  
    [Parameter]
    public int ModuleId { get; set; }


    
    // SERVICES
    

    [Inject]
    private IQuizService QuizService { get; set; } = default!;

    [Inject]
    private AuthState AuthState { get; set; } = default!;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    
    [Inject]
    private ICertificateService CertificateService { get; set; } = default!;


        
    // DATA
   
    private QuizModuleSummary? module;

    private List<QuizQuestionSummary> questions = [];


    // ---- Passing score for this module ----
    private int passingScore = 70;
    private bool isSavingScore;
    private string scoreMessage = string.Empty;


    // STATE
    

    private bool isLoading = true;


    //  Add / Edit question  
    private bool showQuestionModal;
    private bool isSaving;
    private string saveError = string.Empty;

    // 0 = Add a new question.  other value = edit that question.
    private int editingQuestionId;

    private string questionText = string.Empty;
    private QuestionType questionType = QuestionType.MCQ;
    private int marks = 5;
    private string explanation = string.Empty;
    private string imageUrl = string.Empty;

    //  answer options 
    private List<OptionEditRow> optionRows = [];

    //  single correct answer
    private string fillAnswer = string.Empty;

    
    private List<int> originalOptionIds = [];

    //  option row in the modal.
    private class OptionEditRow
    {
        public int OptionId { get; set; }          // 0 = new option
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }


    
    // INITIAL LOAD
  

    protected override async Task OnInitializedAsync()
    {
        if (!AuthState.IsLoggedIn ||
            AuthState.Role != "Content Creator")
        {
            Navigation.NavigateTo("/");
            return;
        }

        await LoadModule();

        await LoadQuestions();

        await LoadPassingScore();
    }



    // LOAD MODULE

    private async Task LoadModule()
    {
        try
        {
            var modules =
                await QuizService.GetModulesAsync();

            module =
                modules.FirstOrDefault(
                    x => x.ModuleId == ModuleId);
        }
        catch
        {
            module = null;
        }
    }


    
    // LOAD QUESTIONS
   

    private async Task LoadQuestions()
    {
        isLoading = true;

        try
        {
            var allQuestions =
                await QuizService.GetQuestionsAsync();

            questions =
                allQuestions
                    .Where(x => x.ModuleId == ModuleId)
                    .OrderBy(x => x.QuestionId)
                    .ToList();
        }
        catch
        {
            questions = [];
        }
        finally
        {
            isLoading = false;
        }
    }


    
    // NAVIGATION
   

    private void BackToModule()
    {
        Navigation.NavigateTo(
            $"/modules/{ModuleId}");
    }


    // QUESTION ACTIONS
 

   
    private bool IsMcq => questionType == QuestionType.MCQ;
    private bool IsTrueFalse => questionType == QuestionType.TrueFalse;
    private bool IsFillBlank => questionType == QuestionType.FillInTheBlank;


    // question Type dropdown
    private void OnQuestionTypeChanged()
    {
        saveError = string.Empty;

        if (IsTrueFalse)
        {
          
            optionRows =
            [
                new OptionEditRow { Text = "True" },
                new OptionEditRow { Text = "False" }
            ];
        }
        else if (IsMcq)
        {
            
            if (optionRows.Count < 2)
            {
                optionRows =
                [
                    new OptionEditRow(),
                    new OptionEditRow()
                ];
            }
        }
      
    }


    private void SetSingleCorrect(OptionEditRow chosen)
    {
        foreach (var row in optionRows)
        {
            row.IsCorrect = row == chosen;
        }
    }


   //create question
    private void OpenCreateQuestionModal()
    {
        editingQuestionId = 0;

        questionText = string.Empty;
        questionType = QuestionType.MCQ;
        marks = 5;
        explanation = string.Empty;
        imageUrl = string.Empty;

       
        optionRows =
        [
            new OptionEditRow(),
            new OptionEditRow()
        ];
        originalOptionIds = [];
        fillAnswer = string.Empty;

        saveError = string.Empty;
        showQuestionModal = true;
    }


    private async Task OpenEditQuestionModal(
        QuizQuestionSummary question)
    {
        editingQuestionId = question.QuestionId;

        questionText = question.QuestionText;
        questionType = question.QuestionType;
        marks = question.Marks;
        explanation = question.Explanation;
        imageUrl = question.ImageUrl;

        // Load this question's existing options into editable rows.
        var allOptions = await QuizService.GetOptionsAsync();

        optionRows = allOptions
            .Where(x => x.QuestionId == question.QuestionId)
            .Select(x => new OptionEditRow
            {
                OptionId = x.OptionId,
                Text = x.OptionText,
                IsCorrect = x.IsCorrect
            })
            .ToList();

        // Remember which options existed, so we know what to delete on save.
        originalOptionIds = optionRows
            .Select(x => x.OptionId)
            .ToList();

        // For Fill-in-the-Blank, show the stored correct answer in its box.
        fillAnswer = optionRows
            .FirstOrDefault(x => x.IsCorrect)?.Text
            ?? optionRows.FirstOrDefault()?.Text
            ?? string.Empty;

        saveError = string.Empty;
        showQuestionModal = true;
    }


    private void AddOptionRow()
    {
        optionRows.Add(new OptionEditRow());
    }


    private void RemoveOptionRow(OptionEditRow row)
    {
        optionRows.Remove(row);
    }


    private void CloseQuestionModal()
    {
        showQuestionModal = false;
        editingQuestionId = 0;
        optionRows = [];
        originalOptionIds = [];
        fillAnswer = string.Empty;
        saveError = string.Empty;
    }


   
    private async Task SaveQuestion()
    {
        
        if (string.IsNullOrWhiteSpace(questionText))
        {
            saveError = "Please enter the question text.";
            return;
        }

        
        List<OptionEditRow> optionsToSave;

        if (IsMcq)
        {
            optionsToSave = optionRows
                .Where(x => !string.IsNullOrWhiteSpace(x.Text))
                .ToList();

            if (optionsToSave.Count < 2)
            {
                saveError = "Please add at least two options.";
                return;
            }

            if (!optionsToSave.Any(x => x.IsCorrect))
            {
                saveError = "Please mark at least one option as correct.";
                return;
            }
        }
        else if (IsTrueFalse)
        {
            
            optionsToSave = optionRows;

            if (!optionsToSave.Any(x => x.IsCorrect))
            {
                saveError = "Please choose whether the answer is True or False.";
                return;
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(fillAnswer))
            {
                saveError = "Please enter the correct answer.";
                return;
            }

            
            optionsToSave =
            [
                new OptionEditRow
                {
                    OptionId = originalOptionIds.FirstOrDefault(),
                    Text = fillAnswer.Trim(),
                    IsCorrect = true
                }
            ];
        }

        isSaving = true;
        saveError = string.Empty;

        try
        {
            var request = new SaveQuizQuestionRequest
            {
                QuestionId = editingQuestionId,
                ModuleId = ModuleId,
                QuestionText = questionText,
                QuestionType = questionType,
                ImageUrl = imageUrl,
                Explanation = explanation,
                Marks = marks
            };

            var result =
                await QuizService.SaveQuestionAsync(request);

            if (!result.Success)
            {
                saveError =
                    string.IsNullOrWhiteSpace(result.Message)
                        ? "Could not save the question. Please try again."
                        : result.Message;
                return;
            }

            
            var questionId =
                editingQuestionId == 0 ? result.Id : editingQuestionId;

            if (questionId != 0)
            {
                await SaveOptionsAsync(questionId, optionsToSave);
            }

            CloseQuestionModal();
            await LoadQuestions();
        }
        catch
        {
            saveError =
                "Something went wrong while saving the question.";
        }
        finally
        {
            isSaving = false;
        }
    }


  
    private async Task SaveOptionsAsync(
        int questionId,
        List<OptionEditRow> filledOptions)
    {
        
        foreach (var row in filledOptions)
        {
            var optionRequest = new SaveQuizOptionRequest
            {
                OptionId = row.OptionId,     // 0 = create, else update
                QuestionId = questionId,
                OptionText = row.Text,
                IsCorrect = row.IsCorrect
            };

            await QuizService.SaveOptionAsync(optionRequest);
        }

        // Delete options 
        var keptIds = filledOptions
            .Select(x => x.OptionId)
            .ToList();

        var removedIds = originalOptionIds
            .Where(id => id != 0 && !keptIds.Contains(id))
            .ToList();

        foreach (var id in removedIds)
        {
            await QuizService.DeleteOptionAsync(id);
        }
    }
   
    // PASSING SCORE
 

    // passing score defaults to 70 
    private async Task LoadPassingScore()
    {
        try
        {
            var setting = await CertificateService.GetSettingAsync(ModuleId);
            if (setting != null)
            {
                passingScore = setting.MinimumPassingScore;
            }
        }
        catch
        {
            // Keeps the default score if it can't be loaded.
        }
    }

    // saves the passing score
    private async Task SavePassingScore()
    {
        isSavingScore = true;
        scoreMessage = string.Empty;

        try
        {  
            var request = new SaveCertificateSettingRequest
            {                                                                     //for certificate
                ModuleId = ModuleId,
                IsEnabled = true,
                Title = "Certificate of Completion",
                Message = string.Empty,
                MinimumPassingScore = passingScore,
                Template = "Classic",
                IssuerName = "Copperleaf"
            };

            var result = await CertificateService.SaveSettingAsync(request);
            scoreMessage = result.Message;
        }
        catch
        {
            scoreMessage = "Something went wrong while saving the score.";
        }
        finally
        {
            isSavingScore = false;
        }
    }


    // Confirms, then deletes the question and reloads the list.
    private async Task DeleteQuestion(
        QuizQuestionSummary question)
    {
        var confirmed = await JS.InvokeAsync<bool>(    
            "confirm",
            "Are you sure you want to delete this question?");

        if (!confirmed)
        {
            return;
        }

        var success =
            await QuizService.DeleteQuestionAsync(question.QuestionId);

        if (success)
        {
            await LoadQuestions();
        }
    }
}