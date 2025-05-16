using FootballLeague.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace FootballLeague.Tests.Helpers
{
    public static class TestHelper
    {
        public static FootballLeagueDbContext CreateContext(string name = null)
        {
            var dbName = name ?? Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<FootballLeagueDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new FootballLeagueDbContext(options);
        }
    }
}
