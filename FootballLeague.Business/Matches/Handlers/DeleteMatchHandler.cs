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
    public class DeleteMatchHandler : IRequestHandler<DeleteMatchCommand, Unit>
    {
        private readonly FootballLeagueDbContext dbContext;

        private readonly IHubContext<RankingHub>? hubContext;

        public DeleteMatchHandler(FootballLeagueDbContext dbContext, IHubContext<RankingHub>? hubContext = null)
        {
            this.dbContext = dbContext;
            this.hubContext = hubContext;
        }

        public async Task<Unit> Handle(DeleteMatchCommand request, CancellationToken ct)
        {
            var match = await this.dbContext.Matches.FindAsync(new object[] { request.Id }, ct);

            if (match is null)
            {
                throw new KeyNotFoundException("Match not found");
            }

            this.dbContext.Matches.Remove(match);
            await this.dbContext.SaveChangesAsync(ct);

            if (this.hubContext?.Clients?.All != null)
            {
                await this.hubContext.Clients.All.SendAsync("MatchDeleted", request.Id, ct);
                await this.hubContext.Clients.All.SendAsync("RankingUpdated", ct);
            }

            return Unit.Value;
        }
    }
}
