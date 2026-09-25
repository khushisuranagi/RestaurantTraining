using RestaurantTraining.Web.Models;

namespace RestaurantTraining.Web.Services.LearnerQuizManagement;

public interface ILearnerQuizService
{
    Task<LearnerQuizModel?> GetQuizAsync(int moduleId);
    Task<QuizResultModel?> SubmitQuizAsync(int moduleId, QuizSubmitRequest request);
}
