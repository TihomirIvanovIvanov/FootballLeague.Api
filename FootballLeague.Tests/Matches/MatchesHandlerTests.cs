using FootballLeague.Business.Matches.Commands;
using FootballLeague.Business.Matches.Handlers;
using FootballLeague.Business.Matches.Queries;
using FootballLeague.Data;
using FootballLeague.Models;
using FootballLeague.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FootballLeague.Tests.Matches
{
    public class MatchesHandlerTests
    {
        private FootballLeagueDbContext GetDb() => TestHelper.CreateContext();

        [Fact]
        public async Task GetAllMatches_Returns_AllMatches()
        {
            var db = GetDb();
            db.Teams.AddRange(new Team { Id = 1, Name = "T1" }, new Team { Id = 2, Name = "T2" });
            db.Matches.AddRange(
                new Match { Id = 1, HomeTeamId = 1, AwayTeamId = 2, HomeScore = 2, AwayScore = 1, Date = DateTime.UtcNow },
                new Match { Id = 2, HomeTeamId = 2, AwayTeamId = 1, HomeScore = 0, AwayScore = 0, Date = DateTime.UtcNow }
            );
            await db.SaveChangesAsync();

            var handler = new GetAllMatchesHandler(db);

            var result = await handler.Handle(new GetAllMatchesQuery(), CancellationToken.None);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetMatchById_Existing_Returns_MatchDto()
        {
            var db = GetDb();
            db.Teams.AddRange(new Team { Id = 1, Name = "T1" }, new Team { Id = 2, Name = "T2" });
            db.Matches.Add(new Match { Id = 5, HomeTeamId = 1, AwayTeamId = 2, HomeScore = 1, AwayScore = 2, Date = DateTime.UtcNow });
            await db.SaveChangesAsync();

            var handler = new GetMatchByIdHandler(db);

            var dto = await handler.Handle(new GetMatchByIdQuery(5), CancellationToken.None);

            Assert.Equal(1, dto.HomeScore);
            Assert.Equal("T2", dto.AwayTeam);
        }

        [Fact]
        public async Task GetMatchById_NonExisting_Throws_KeyNotFoundException()
        {
            var handler = new GetMatchByIdHandler(GetDb());
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => handler.Handle(new GetMatchByIdQuery(99), CancellationToken.None));
        }

        [Fact]
        public async Task CreateMatch_Adds_New_Match()
        {
            var db = GetDb();
            db.Teams.AddRange(new Team { Id = 1, Name = "T1" }, new Team { Id = 2, Name = "T2" });
            await db.SaveChangesAsync();

            var handler = new CreateMatchHandler(db);

            var mId = await handler.Handle(new CreateMatchCommand(1, 2, 3, 3, DateTime.UtcNow), CancellationToken.None);
            var created = await db.Matches.FindAsync(new object[] { mId }, CancellationToken.None);

            Assert.NotNull(created);
            Assert.Equal(3, created.HomeScore);
        }

        [Fact]
        public async Task CreateMatch_RetiredHomeTeam_ThrowsValidationException()
        {
            var db = GetDb();
            db.Teams.AddRange(
                new Team { Id = 1, Name = "T1", IsRetired = true },
                new Team { Id = 2, Name = "T2", IsRetired = false }
            );
            await db.SaveChangesAsync();

            var handler = new CreateMatchHandler(db);
            var cmd = new CreateMatchCommand(
                homeTeamId: 1,
                awayTeamId: 2,
                homeScore: 0,
                awayScore: 0,
                date: DateTime.UtcNow
            );

            await Assert.ThrowsAsync<ValidationException>(() =>
                handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task CreateMatch_SameTeamIds_ThrowsValidationException()
        {
            var db = GetDb();
            db.Teams.Add(new Team { Id = 5, Name = "Same", IsRetired = false });
            await db.SaveChangesAsync();

            var handler = new CreateMatchHandler(db);
            var cmd = new CreateMatchCommand(
                homeTeamId: 5,
                awayTeamId: 5,
                homeScore: 1,
                awayScore: 1,
                date: DateTime.UtcNow
            );

            var ex = await Assert.ThrowsAsync<ValidationException>(
                () => handler.Handle(cmd, CancellationToken.None));
            Assert.Contains("must be different", ex.Message);
        }

        [Fact]
        public async Task UpdateMatch_Existing_Updates_Scores()
        {
            var db = GetDb();
            db.Teams.AddRange(new Team { Id = 1, Name = "T1" }, new Team { Id = 2, Name = "T2" });
            db.Matches.Add(new Match { Id = 7, HomeTeamId = 1, AwayTeamId = 2, HomeScore = 0, AwayScore = 0, Date = DateTime.UtcNow });
            await db.SaveChangesAsync();

            var handler = new UpdateMatchHandler(db);

            await handler.Handle(new UpdateMatchCommand(7, 5, 4), CancellationToken.None);
            var match = await db.Matches.FindAsync(new object[] { 7 }, CancellationToken.None);

            Assert.Equal(5, match.HomeScore);
        }

        [Fact]
        public async Task UpdateMatch_NonExisting_Throws_KeyNotFoundException()
        {
            var handler = new UpdateMatchHandler(GetDb());
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => handler.Handle(new UpdateMatchCommand(99, 1, 1), CancellationToken.None));
        }

        [Fact]
        public async Task DeleteMatch_Existing_RemovesMatch()
        {
            var db = GetDb();
            db.Teams.AddRange(new Team { Id = 1, Name = "T1" }, new Team { Id = 2, Name = "T2" });
            db.Matches.Add(new Match { Id = 7, HomeTeamId = 1, AwayTeamId = 2, HomeScore = 0, AwayScore = 0, Date = DateTime.UtcNow });
            await db.SaveChangesAsync();

            var handler = new DeleteMatchHandler(db);

            await handler.Handle(new DeleteMatchCommand(7), CancellationToken.None);
            var exists = await db.Matches.AnyAsync(m => m.Id == 7, CancellationToken.None);

            Assert.False(exists);
        }

        [Fact]
        public async Task DeleteMatch_NonExisting_Throws_KeyNotFoundException()
        {
            var handler = new DeleteMatchHandler(GetDb());
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => handler.Handle(new DeleteMatchCommand(99), CancellationToken.None));
        }

        [Fact]
        public void CreateMatchCommand_Invalid_FutureDate_Throws_ValidateDateNotInFuture()
        {
            var cmd = new CreateMatchCommand(1, 2, 0, 0, DateTime.UtcNow.AddHours(1));
            var context = new ValidationContext(cmd);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(cmd, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, vr => vr.ErrorMessage.Contains("Date cannot be in the future."));
        }
    }
}
