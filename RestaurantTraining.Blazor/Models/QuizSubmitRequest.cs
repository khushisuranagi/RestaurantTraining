namespace RestaurantTraining.Blazor.Models;

public class QuizSubmitRequest
{
    public List<QuizAnswerModel> Answers { get; set; } = [];
}

public class QuizAnswerModel
{
    public int QuestionId { get; set; }
    public List<int> SelectedOptionIds { get; set; } = [];
    public string? TextAnswer { get; set; }
}