using MediatR;

namespace RestaurantTraining.Application.Features.LearnerScenario.Commands.ReplyToScenario
{
    // One chat turn: the learner sends the full conversation so far (their new
    // message last); the AI customer replies and grades.
    public class ReplyToScenarioCommand : IRequest<ReplyToScenarioResult>
    {
        public int ScenarioId { get; set; }
        public int UserId { get; set; }
        public List<ScenarioMessageInput> Messages { get; set; } = [];
    }

    public class ScenarioMessageInput
    {
        public bool FromLearner { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class ReplyToScenarioResult
    {
        public bool NotAvailable { get; set; }   // → controller 404

        public string Reply { get; set; } = string.Empty;   // customer's next message
        public string Feedback { get; set; } = string.Empty;

        public bool Ended { get; set; }          // conversation finished (pass or cap reached)
        public bool Passed { get; set; }         // customer satisfied
        public bool CertificateIssued { get; set; }
    }
}
