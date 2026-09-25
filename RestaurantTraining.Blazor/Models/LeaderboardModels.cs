namespace RestaurantTraining.Web.Models;

public class LeaderboardModel
{
    public List<LeaderboardEntryModel> Top { get; set; } = [];
    public LeaderboardEntryModel? Me { get; set; }
}

public class LeaderboardEntryModel
{
    public int Rank { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public int Points { get; set; }
    public bool IsOutstanding { get; set; }
}