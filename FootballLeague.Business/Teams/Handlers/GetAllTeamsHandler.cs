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
    public class GetAllTeamsHandler : IRequestHandler<GetAllTeamsQuery, List<TeamDto>>
    {
        private readonly FootballLeagueDbContext dbContext;

        public GetAllTeamsHandler(FootballLeagueDbContext db)
        {
            this.dbContext = db;
        }

        public async Task<List<TeamDto>> Handle(GetAllTeamsQuery request, CancellationToken ct)
        {
            return await this.dbContext.Teams
                .Where(t => !t.IsRetired)
                .Select(t => new TeamDto { Id = t.Id, Name = t.Name })
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
