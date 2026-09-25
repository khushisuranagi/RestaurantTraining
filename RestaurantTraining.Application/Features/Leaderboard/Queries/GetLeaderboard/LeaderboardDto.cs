namespace RestaurantTraining.Application.Features.Leaderboard.Queries.GetLeaderboard
{
    public class LeaderboardDto
    {
        public List<LeaderboardEntryDto> Top { get; set; } = [];
        public LeaderboardEntryDto? Me { get; set; }   // the caller's own rank (null if unranked)
    }

    public class LeaderboardEntryDto
    {
        public int Rank { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public int Points { get; set; }
        public bool IsOutstanding { get; set; }   // top 3
    }
}