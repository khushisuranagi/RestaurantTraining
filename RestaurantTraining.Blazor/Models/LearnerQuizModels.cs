namespace RestaurantTraining.Web.Models;

public class LearnerQuizModel
{
    public int ModuleId { get; set; }
    public List<LearnerQuizQuestion> Questions { get; set; } = [];
}

public class LearnerQuizQuestion
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public int QuestionType { get; set; }   // 0 = MCQ, 1 = True/False, 2 = Fill-in-the-Blank
    public int Marks { get; set; }
    public bool AllowMultipleAnswers { get; set; }
    public List<LearnerQuizOption> Options { get; set; } = [];
}

public class LearnerQuizOption
{
    public int OptionId { get; set; }
    public string OptionText { get; set; } = string.Empty;
}