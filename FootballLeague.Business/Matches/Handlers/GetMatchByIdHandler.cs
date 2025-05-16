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
    public class GetMatchByIdHandler : IRequestHandler<GetMatchByIdQuery, MatchDto>
    {
        private readonly FootballLeagueDbContext dbContext;

        public GetMatchByIdHandler(FootballLeagueDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<MatchDto> Handle(GetMatchByIdQuery request, CancellationToken ct)
        {
            var match = await this.dbContext.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(m => m.Id == request.Id)
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
                .FirstOrDefaultAsync(ct);

            if (match == null)
            {
                throw new KeyNotFoundException("Match not found");
            }
            return match;
        }
    }
}
