using FootballLeague.Business.Teams.Commands;
using FootballLeague.Data;
using FootballLeague.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Business.Teams.Handlers
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, int>
    {
        private readonly FootballLeagueDbContext dbContext;

        public CreateTeamHandler(FootballLeagueDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> Handle(CreateTeamCommand request, CancellationToken ct)
        {
            var team = new Team { Name = request.Name };
            this.dbContext.Teams.Add(team);
            await this.dbContext.SaveChangesAsync(ct);
            return team.Id;
        }
    }
}
