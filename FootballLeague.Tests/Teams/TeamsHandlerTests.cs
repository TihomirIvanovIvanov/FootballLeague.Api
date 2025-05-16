using FootballLeague.Business.Teams.Commands;
using FootballLeague.Business.Teams.Handlers;
using FootballLeague.Business.Teams.Queries;
using FootballLeague.Data;
using FootballLeague.Models;
using FootballLeague.Tests.Helpers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FootballLeague.Tests.Teams
{
    public class TeamsHandlerTests
    {
        private FootballLeagueDbContext GetDb() => TestHelper.CreateContext();

        [Fact]
        public async Task GetAllTeams_Returns_AllSeededTeams()
        {
            var db = GetDb();
            db.Teams.AddRange(new Team { Id = 1, Name = "T1" }, new Team { Id = 2, Name = "T2" });
            await db.SaveChangesAsync();

            var handler = new GetAllTeamsHandler(db);

            var result = await handler.Handle(new GetAllTeamsQuery(), CancellationToken.None);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.Name == "T1");
        }

        [Fact]
        public async Task GetTeamById_Existing_Returns_Team()
        {
            var db = GetDb();
            db.Teams.Add(new Team { Id = 5, Name = "T1" });
            await db.SaveChangesAsync();

            var handler = new GetTeamByIdHandler(db);

            var result = await handler.Handle(new GetTeamByIdQuery(5), CancellationToken.None);

            Assert.Equal("T1", result.Name);
        }

        [Fact]
        public async Task GetTeamById_NonExisting_Throws_KeyNotFoundException()
        {
            var handler = new GetTeamByIdHandler(GetDb());
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => handler.Handle(new GetTeamByIdQuery(99), CancellationToken.None));
        }

        [Fact]
        public async Task CreateTeam_Adds_New_Team()
        {
            var db = GetDb();
            var handler = new CreateTeamHandler(db);

            var newId = await handler.Handle(new CreateTeamCommand("T1"), CancellationToken.None);
            var added = await db.Teams.FindAsync(new object[] { newId }, CancellationToken.None);

            Assert.NotNull(added);
            Assert.Equal("T1", added.Name);
        }

        [Fact]
        public async Task UpdateTeam_Existing_Updates_Name()
        {
            var db = GetDb();
            db.Teams.Add(new Team { Id = 10, Name = "OldName" });
            await db.SaveChangesAsync();

            var handler = new UpdateTeamHandler(db);

            await handler.Handle(new UpdateTeamCommand(10, "NewName"), CancellationToken.None);
            var updated = await db.Teams.FindAsync(new object[] { 10 }, CancellationToken.None);

            Assert.Equal("NewName", updated.Name);
        }

        [Fact]
        public async Task UpdateTeam_NonExisting_Throws_KeyNotFoundException()
        {
            var handler = new UpdateTeamHandler(GetDb());
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => handler.Handle(new UpdateTeamCommand(42, "T1"), CancellationToken.None));
        }

        [Fact]
        public async Task DeleteTeam_Existing_Marks_Team_As_Retired()
        {
            var db = GetDb();
            var team = new Team { Id = 20, Name = "ToDelete" };
            db.Teams.Add(team);
            await db.SaveChangesAsync();

            var handler = new DeleteTeamHandler(db);

            await handler.Handle(new DeleteTeamCommand(20), CancellationToken.None);

            var retiredTeam = await db.Teams.FindAsync(new object[] { 20 }, CancellationToken.None);
            Assert.NotNull(retiredTeam);
            Assert.True(retiredTeam.IsRetired,
                "Team should be marked as retired instead of being deleted from the database.");
        }

        [Fact]
        public async Task DeleteTeam_NonExisting_Throws_KeyNotFoundException()
        {
            var handler = new DeleteTeamHandler(GetDb());
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => handler.Handle(new DeleteTeamCommand(99), CancellationToken.None));
        }
    }
}
