using FootballLeague.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FootballLeague.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FootballLeagueDbContext>();

            dbContext.Database.Migrate();

            if (!dbContext.Teams.Any())
            {
                var teams = new[]
                {
                    new Team { Name = "Aristocrat" },
                    new Team { Name = "Pariplay" },
                    new Team { Name = "Wizard" }
                };
                dbContext.Teams.AddRange(teams);
                dbContext.SaveChanges();

                var matches = new[]
                {
                    new Match { HomeTeamId = teams[0].Id, AwayTeamId = teams[1].Id, HomeScore = 2, AwayScore = 1, Date = DateTime.UtcNow.AddDays(-7) },
                    new Match { HomeTeamId = teams[1].Id, AwayTeamId = teams[2].Id, HomeScore = 3, AwayScore = 3, Date = DateTime.UtcNow.AddDays(-5) },
                    new Match { HomeTeamId = teams[2].Id, AwayTeamId = teams[0].Id, HomeScore = 0, AwayScore = 4, Date = DateTime.UtcNow.AddDays(-3) }
                };
                dbContext.Matches.AddRange(matches);
                dbContext.SaveChanges();
            }
        }
    }
}
