using MediatR;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.Application.Features.Leaderboard.Queries.GetLeaderboard
{
    public class GetLeaderboardQueryHandler
        : IRequestHandler<GetLeaderboardQuery, LeaderboardDto>
    {
        private readonly ILeaderboardRepository _repository;

        public GetLeaderboardQueryHandler(ILeaderboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<LeaderboardDto> Handle(
            GetLeaderboardQuery request, CancellationToken cancellationToken)
        {
            var ranked = await _repository.GetRankedLearnersAsync(cancellationToken);

            // The list is already ordered, so rank = position + 1.
            var entries = ranked
                .Select((row, i) => new LeaderboardEntryDto
                {
                    Rank = i + 1,
                    FullName = row.FullName,
                    RoleName = row.RoleName,
                    Points = row.Points,
                    IsOutstanding = i < 3          // top 3
                })
                .ToList();

            var top = entries.Take(request.Take).ToList();

            LeaderboardEntryDto? me = null;
            if (request.UserId is int uid)
            {
                var idx = ranked.FindIndex(r => r.UserId == uid);
                if (idx >= 0) me = entries[idx];
            }

            return new LeaderboardDto { Top = top, Me = me };
        }
    }
}