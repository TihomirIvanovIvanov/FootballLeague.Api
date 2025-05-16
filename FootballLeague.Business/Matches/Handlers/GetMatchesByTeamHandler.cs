using FootballLeague.Business.Matches.Dtos;
using FootballLeague.Business.Matches.Queries;
using FootballLeague.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Business.Matches.Handlers
{
    public class GetMatchesByTeamHandler : IRequestHandler<GetMatchesByTeamQuery, List<MatchDto>>
    {
        private readonly FootballLeagueDbContext dbContext;

        public GetMatchesByTeamHandler(FootballLeagueDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<MatchDto>> Handle(GetMatchesByTeamQuery request, CancellationToken ct)
        {
            var matches = await this.dbContext.Matches
                .Where(m => m.HomeTeamId == request.TeamId ||
                            m.AwayTeamId == request.TeamId)
                .Select(m => new MatchDto
                {
                    MatchId = m.Id,
                    Date = m.Date,
                    HomeTeam = m.HomeTeam.Name,
                    AwayTeam = m.AwayTeam.Name,
                    HomeScore = m.HomeScore,
                    AwayScore = m.AwayScore
                })
                .AsNoTracking()
                .ToListAsync(ct);

            if (matches == null || matches.Count == 0)
            {
                throw new KeyNotFoundException("This team has no matches");
            }
            return matches;
        }
    }
}
