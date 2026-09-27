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

    // Rank-based award label: 1st = Outstanding, 2nd = Excellent, 3rd = Good.
    public string AwardLabel => Rank switch
    {
        1 => "Outstanding",
        2 => "Excellent",
        3 => "Good",
        _ => string.Empty
    };
}