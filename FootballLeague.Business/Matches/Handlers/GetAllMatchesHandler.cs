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
    public class GetAllMatchesHandler : IRequestHandler<GetAllMatchesQuery, List<MatchDto>>
    {
        private readonly FootballLeagueDbContext dbContext;

        public GetAllMatchesHandler(FootballLeagueDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<MatchDto>> Handle(GetAllMatchesQuery request, CancellationToken ct)
        {
            return await dbContext.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Select(m => new MatchDto
                {
                    AwayScore = m.AwayScore,
                    AwayTeam = m.AwayTeam.Name,
                    Date = m.Date,
                    HomeScore = m.HomeScore,
                    HomeTeam = m.HomeTeam.Name,
                    MatchId = m.Id,
                })
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
