namespace RestaurantTraining.Blazor.Models;

// The AI MCQ shown under a lesson resource (no correct answer included).
public class ResourceQuestionModel
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public bool AlreadyAnsweredCorrectly { get; set; }
    public List<ResourceQuestionOptionModel> Options { get; set; } = [];
}

public class ResourceQuestionOptionModel
{
    public int OptionId { get; set; }
    public string OptionText { get; set; } = string.Empty;
}

public class ResourceAnswerRequest
{
    public int SelectedOptionId { get; set; }
}

public class ResourceAnswerResultModel
{
    public bool IsCorrect { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public int PointsAwarded { get; set; }
    public int TotalPoints { get; set; }
}
