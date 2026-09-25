namespace RestaurantTraining.Application.Common.Interfaces
{
    public interface ILeaderboardRepository
    {
        // All qualifying learners (active, non-creator, points > 0), already
        // ordered best-first. The handler assigns ranks from this order.
        Task<List<LeaderboardRow>> GetRankedLearnersAsync(CancellationToken cancellationToken);
    }

    public class LeaderboardRow
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public int Points { get; set; }
        public DateTime LastAnsweredAt { get; set; }   // tie-break only
    }
}