using FootballLeague.Business.Services;
using FootballLeague.Business.Teams.Dtos;
using FootballLeague.Business.Teams.Handlers;
using FootballLeague.Business.Teams.Queries;
using FootballLeague.Data;
using FootballLeague.Models;
using FootballLeague.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Match = FootballLeague.Models.Match;

namespace FootballLeague.Tests.Teams
{
    public class TeamRankingHandlerTests
    {
        private FootballLeagueDbContext GetDb() => TestHelper.CreateContext();

        [Fact]
        public async Task Handle_UsesScoringService_ForEachTeam()
        {
            var db = GetDb();
            db.Teams.Add(new Team { Id = 42, Name = "Mockers" });
            db.SaveChanges();

            var mockScoring = new Mock<IScoringService>();
            mockScoring
                .Setup(s => s.CalculateTeamRanking(It.Is<Team>(t => t.Id == 42)))
                .Returns(new TeamRankingDto
                {
                    TeamId = 42,
                    TeamName = "Mockers",
                    Played = 0,
                    Won = 0,
                    Drawn = 0,
                    Lost = 0,
                    Points = 0
                });

            var handler = new GetTeamRankingHandler(db, mockScoring.Object);
            var result = await handler.Handle(new GetTeamRankingQuery(),
                                              CancellationToken.None);
            mockScoring.Verify(s =>
                s.CalculateTeamRanking(It.IsAny<Team>()), Times.Once);

            Assert.Single(result);
            Assert.Equal("Mockers", result.First().TeamName);
        }

        [Fact]
        public async Task Ranking_Calculates_PointsAndOrder()
        {
            var db = GetDb();
            db.Teams.AddRange(new Team { Id = 1, Name = "T1" }, new Team { Id = 2, Name = "T2" });
            db.Matches.AddRange(
                new Match { HomeTeamId = 1, AwayTeamId = 2, HomeScore = 2, AwayScore = 0, Date = DateTime.UtcNow },
                new Match { HomeTeamId = 1, AwayTeamId = 2, HomeScore = 1, AwayScore = 1, Date = DateTime.UtcNow }
            );
            await db.SaveChangesAsync();

            var handler = new GetTeamRankingHandler(db, new ScoringService());
            var result = await handler.Handle(new GetTeamRankingQuery(), CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Equal("T1", result[0].TeamName);     // Team1: win+draw = 4 points
            Assert.Equal(4, result[0].Points);
            Assert.Equal("T2", result[1].TeamName);     // Team2: loss+draw = 1 point
            Assert.Equal(1, result[1].Points);
        }

        [Fact]
        public async Task Handle_NoTeams_ThrowsKeyNotFound()
        {
            var db = GetDb();
            var scoring = new Mock<IScoringService>().Object;
            var handler = new GetTeamRankingHandler(db, scoring);

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => handler.Handle(new GetTeamRankingQuery(), CancellationToken.None));
        }

        [Fact]
        public async Task Handle_EqualPoints_OrderByTeamName()
        {
            var db = GetDb();

            db.Teams.AddRange
            (
                new Team { Id = 1, Name = "T2" },
                new Team { Id = 2, Name = "T1" }
            );
            db.Matches.Add(new Match { HomeTeamId = 1, AwayTeamId = 2, HomeScore = 1, AwayScore = 1, Date = DateTime.UtcNow });
            await db.SaveChangesAsync();

            var handler = new GetTeamRankingHandler(db, new ScoringService());
            var result = await handler.Handle(new GetTeamRankingQuery(), CancellationToken.None);

            Assert.Equal("T1", result[0].TeamName);
            Assert.Equal("T2", result[1].TeamName);
        }

        [Fact]
        public async Task Handle_AllLosses_ZeroPoints()
        {
            var db = GetDb();
            db.Teams.Add(new Team { Id = 1, Name = "Loser" });
            db.Teams.Add(new Team { Id = 2, Name = "Winner" });
            db.Matches.AddRange(
                new Match { HomeTeamId = 2, AwayTeamId = 1, HomeScore = 2, AwayScore = 0, Date = DateTime.UtcNow },
                new Match { HomeTeamId = 2, AwayTeamId = 1, HomeScore = 3, AwayScore = 1, Date = DateTime.UtcNow }
            );
            await db.SaveChangesAsync();

            var result = await new GetTeamRankingHandler(db, new ScoringService())
                .Handle(new GetTeamRankingQuery(), CancellationToken.None);

            var loser = result.Single(r => r.TeamName == "Loser");
            Assert.Equal(0, loser.Points);
        }
    }
}
