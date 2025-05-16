using FootballLeague.Business.Hubs;
using FootballLeague.Business.Matches.Commands;
using FootballLeague.Data;
using FootballLeague.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Business.Matches.Handlers
{
    public class CreateMatchHandler : IRequestHandler<CreateMatchCommand, int>
    {
        private readonly FootballLeagueDbContext dbContext;

        private readonly IHubContext<RankingHub>? hubContext;

        public CreateMatchHandler(FootballLeagueDbContext dbContext, IHubContext<RankingHub>? hubContext = null)
        {
            this.dbContext = dbContext;
            this.hubContext = hubContext;
        }

        public async Task<int> Handle(CreateMatchCommand request, CancellationToken ct)
        {
            if (request.HomeTeamId == request.AwayTeamId)
            {
                throw new ValidationException("HomeTeamId and AwayTeamId must be different.");
            }

            var home = await this.dbContext.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.HomeTeamId, ct);
            if (home is null)
            {
                throw new KeyNotFoundException($"Home team {request.HomeTeamId} not found");
            }
            if (home.IsRetired)
            {
                throw new ValidationException($"Cannot create a match for retired team '{home.Name}' (ID {home.Id})");
            }

            var away = await this.dbContext.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.AwayTeamId, ct);
            if (away is null)
            {
                throw new KeyNotFoundException($"Away team {request.AwayTeamId} not found");
            }
            if (away.IsRetired)
            {
                throw new ValidationException($"Cannot create a match for retired team '{away.Name}' (ID {away.Id})");
            }

            var match = new Match
            {
                HomeTeamId = request.HomeTeamId,
                AwayTeamId = request.AwayTeamId,
                HomeScore = request.HomeScore,
                AwayScore = request.AwayScore,
                Date = request.Date
            };

            this.dbContext.Matches.Add(match);
            await this.dbContext.SaveChangesAsync(ct);

            if (this.hubContext?.Clients?.All != null)
            {
                await this.hubContext.Clients.All.SendAsync(
                "MatchCreated", new
                {
                    match.Id,
                    match.HomeTeamId,
                    match.AwayTeamId,
                    match.HomeScore,
                    match.AwayScore,
                    match.Date
                },
                ct);

                await this.hubContext.Clients.All.SendAsync("RankingUpdated", ct);
            }

            return match.Id;
        }
    }
}
