using FootballLeague.Business.Hubs;
using FootballLeague.Business.Matches.Commands;
using FootballLeague.Data;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Business.Matches.Handlers
{
    public class UpdateMatchHandler : IRequestHandler<UpdateMatchCommand, Unit>
    {
        private readonly FootballLeagueDbContext dbContext;

        private readonly IHubContext<RankingHub>? hubContext;

        public UpdateMatchHandler(FootballLeagueDbContext dbContext, IHubContext<RankingHub>? hubContext = null)
        {
            this.dbContext = dbContext;
            this.hubContext = hubContext;
        }

        public async Task<Unit> Handle(UpdateMatchCommand request, CancellationToken ct)
        {
            var match = await this.dbContext.Matches.FindAsync(new object[] { request.Id }, ct);

            if (match is null)
            {
                throw new KeyNotFoundException("Match not found");
            }

            match.HomeScore = request.HomeScore;
            match.AwayScore = request.AwayScore;
            await this.dbContext.SaveChangesAsync(ct);

            if (this.hubContext?.Clients?.All != null)
            {
                await this.hubContext.Clients.All.SendAsync(
                "MatchUpdated", new
                {
                    match.Id,
                    match.HomeTeamId,
                    match.AwayTeamId,
                    match.HomeScore,
                    match.AwayScore,
                    match.Date
                }, ct);

                await this.hubContext.Clients.All.SendAsync("RankingUpdated", ct);
            }
            return Unit.Value;
        }
    }
}
