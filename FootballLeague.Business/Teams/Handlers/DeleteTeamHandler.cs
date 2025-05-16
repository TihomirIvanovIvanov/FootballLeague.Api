using FootballLeague.Business.Teams.Commands;
using FootballLeague.Data;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Business.Teams.Handlers
{
    public class DeleteTeamHandler : IRequestHandler<DeleteTeamCommand, Unit>
    {
        private readonly FootballLeagueDbContext dbContext;

        public DeleteTeamHandler(FootballLeagueDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteTeamCommand request, CancellationToken ct)
        {
            var team = await this.dbContext.Teams.FindAsync(new object[] { request.Id }, ct);

            if (team == null)
            {
                throw new KeyNotFoundException("Team not found");
            }

            team.IsRetired = true;
            await this.dbContext.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
