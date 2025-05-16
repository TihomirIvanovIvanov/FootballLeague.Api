using FootballLeague.Business.Services;
using FootballLeague.Business.Teams.Dtos;
using FootballLeague.Business.Teams.Queries;
using FootballLeague.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Business.Teams.Handlers
{
    public class GetTeamRankingHandler : IRequestHandler<GetTeamRankingQuery, List<TeamRankingDto>>
    {
        private readonly FootballLeagueDbContext dbContext;

        private readonly IScoringService scoringService;

        public GetTeamRankingHandler(FootballLeagueDbContext dbContext, IScoringService scoringService)
        {
            this.dbContext = dbContext;
            this.scoringService = scoringService;
        }

        public async Task<List<TeamRankingDto>> Handle(GetTeamRankingQuery request, CancellationToken ct)
        {
            var teams = await dbContext.Teams
                .Include(t => t.HomeMatches)
                .Include(t => t.AwayMatches)
                .AsNoTracking()
                .ToListAsync(ct);

            if (!teams.Any())
            {
                throw new KeyNotFoundException("No teams found");
            }

            var rankings = teams
                .Select(t => scoringService.CalculateTeamRanking(t))
                .OrderByDescending(r => r.Points)
                .ThenBy(t => t.TeamName)
                .ToList();

            return rankings;
        }
    }
}
