using Microsoft.EntityFrameworkCore;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Persistence.Repositories
{
    public class LeaderboardRepository : ILeaderboardRepository
    {
        private readonly AppDbContext _context;

        public LeaderboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LeaderboardRow>> GetRankedLearnersAsync(
            CancellationToken cancellationToken)
        {
            // 1) Sum points per learner (+ newest scoring answer, for tie-breaks).
            var pointsByUser = await _context.ResourceQuestionAttempts
                .GroupBy(a => a.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    Points = g.Sum(x => x.PointsAwarded),
                    LastAnsweredAt = g.Max(x => x.AnsweredAt)
                })
                .ToListAsync(cancellationToken);

            var userIds = pointsByUser.Select(p => p.UserId).ToList();

            // 2) Names + roles for those users — active learners only (no creators).
            var users = await (
                from u in _context.Users
                join r in _context.Roles on u.RoleId equals r.RoleId
                where userIds.Contains(u.UserId)
                      && u.IsActive
                      && r.RoleName != "Content Creator"
                select new { u.UserId, u.FullName, RoleName = r.RoleName }
            ).ToListAsync(cancellationToken);

            var usersById = users.ToDictionary(u => u.UserId);

            // 3) Combine, drop 0-point / non-learner rows, order best-first.
            return pointsByUser
                .Where(p => p.Points > 0 && usersById.ContainsKey(p.UserId))
                .Select(p => new LeaderboardRow
                {
                    UserId = p.UserId,
                    FullName = usersById[p.UserId].FullName,
                    RoleName = usersById[p.UserId].RoleName,
                    Points = p.Points,
                    LastAnsweredAt = p.LastAnsweredAt
                })
                .OrderByDescending(x => x.Points)
                .ThenBy(x => x.LastAnsweredAt)   // reached the total earlier ranks higher
                .ToList();
        }
    }
}