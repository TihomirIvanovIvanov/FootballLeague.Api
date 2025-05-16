using FootballLeague.Business.Teams.Commands;
using FootballLeague.Data;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Business.Teams.Handlers
{
    public class UpdateTeamHandler : IRequestHandler<UpdateTeamCommand, Unit>
    {
        private readonly FootballLeagueDbContext dbContext;

        public UpdateTeamHandler(FootballLeagueDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateTeamCommand request, CancellationToken ct)
        {
            var team = await this.dbContext.Teams.FindAsync(new object[] { request.Id }, ct);

            if (team == null)
            {
                throw new KeyNotFoundException("Team not found");
            }

            team.Name = request.Name;
            await this.dbContext.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
