using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.QuizManagement;

public interface IQuizService
{
    //Read 
    Task<List<QuizModuleSummary>> GetModulesAsync();

    Task<List<QuizQuestionSummary>> GetQuestionsAsync();

    Task<List<QuizOptionSummary>> GetOptionsAsync();

    //  create / update / delete q's
    Task<QuizSaveResult> SaveQuestionAsync(SaveQuizQuestionRequest request);

    Task<bool> DeleteQuestionAsync(int questionId);

    //create / update / delete options
    Task<QuizSaveResult> SaveOptionAsync(SaveQuizOptionRequest request);

    Task<bool> DeleteOptionAsync(int optionId);
}
