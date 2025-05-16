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
    public class GetTeamByIdHandler : IRequestHandler<GetTeamByIdQuery, TeamDto>
    {
        private readonly FootballLeagueDbContext dbContext;

        public GetTeamByIdHandler(FootballLeagueDbContext db)
        {
            this.dbContext = db;
        }

        public async Task<TeamDto> Handle(GetTeamByIdQuery request, CancellationToken ct)
        {
            var team = await dbContext.Teams
                .Where(t => t.Id == request.Id && !t.IsRetired)
                .Select(t => new TeamDto { Id = t.Id, Name = t.Name })
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);

            if (team is null)
            {
                throw new KeyNotFoundException("Team not found");
            }
            return team;
        }
    }
}
