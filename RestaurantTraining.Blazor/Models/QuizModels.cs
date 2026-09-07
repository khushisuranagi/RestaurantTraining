using RestaurantTraining.Domain.Enums;

namespace RestaurantTraining.Blazor.Models;

public class QuizModuleSummary
{
    public int ModuleId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class QuizQuestionSummary
{
    public int QuestionId { get; set; }
    public int ModuleId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType QuestionType { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public int Marks { get; set; }
}

public class QuizOptionSummary
{
    public int OptionId { get; set; }
    public int QuestionId { get; set; }
    public string OptionText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}

// Used for BOTH create and update of a question (QuestionId == 0 means create).
public class SaveQuizQuestionRequest
{
    public int QuestionId { get; set; }
    public int ModuleId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType QuestionType { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public int Marks { get; set; }
}

// Used for BOTH create and update of an option (OptionId == 0 means create).
public class SaveQuizOptionRequest
{
    public int OptionId { get; set; }
    public int QuestionId { get; set; }
    public string OptionText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}

// Id carries the new record's id after a successful create.
public class QuizSaveResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Id { get; set; }
}
