namespace RestaurantTraining.Web.Models;

// The practice scenario shown to the learner.
public class ScenarioModel
{
    public int ScenarioId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool AlreadyPassed { get; set; }
}

// One chat message in the practice conversation.
public class ScenarioChatMessage
{
    public bool FromLearner { get; set; }
    public string Text { get; set; } = string.Empty;
}

// Sent to the API each turn (the whole conversation so far).
public class ScenarioReplyRequest
{
    public List<ScenarioChatMessage> Messages { get; set; } = [];
}

// The API's reply for one turn.
public class ScenarioReplyResult
{
    public string Reply { get; set; } = string.Empty;
    public string Feedback { get; set; } = string.Empty;
    public bool Ended { get; set; }
    public bool Passed { get; set; }
    public bool CertificateIssued { get; set; }
}
