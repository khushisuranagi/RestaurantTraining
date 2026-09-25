using MediatR;

namespace RestaurantTraining.Application.Features.Leaderboard.Queries.GetLeaderboard
{
    public class GetLeaderboardQuery : IRequest<LeaderboardDto>
    {
        // The caller's user id — used to build their personal "your rank" row.
        // Null for content creators (they don't have a rank).
        public int? UserId { get; set; }
        public int Take { get; set; } = 10;
    }
}