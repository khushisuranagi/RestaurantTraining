using RestaurantTraining.Blazor.Models;

namespace RestaurantTraining.Blazor.Services.LearnerQuizManagement;

public interface ILearnerQuizService
{
    Task<LearnerQuizModel?> GetQuizAsync(int moduleId);
    Task<QuizResultModel?> SubmitQuizAsync(int moduleId, QuizSubmitRequest request);
}
