namespace RestaurantTraining.Blazor.Models;

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
    public List<LearnerQuizOption> Options { get; set; } = [];
}

public class LearnerQuizOption
{
    public int OptionId { get; set; }
    public string OptionText { get; set; } = string.Empty;
}

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

public class LearnerCertificateModel
{
    public int CertificateId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public string LearnerName { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public decimal? ScorePercent { get; set; }
}

public class QuizResultModel
{
    public decimal Score { get; set; }
    public decimal TotalMarks { get; set; }
    public decimal Percentage { get; set; }
    public int PassingScore { get; set; }
    public bool Passed { get; set; }
    public int CorrectCount { get; set; }
    public int TotalQuestions { get; set; }
    public bool CertificateIssued { get; set; }
    public string? CertificateNumber { get; set; }
    public string ModuleName { get; set; } = string.Empty;
}
