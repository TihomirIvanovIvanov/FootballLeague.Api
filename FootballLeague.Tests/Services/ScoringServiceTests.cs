using FootballLeague.Business.Services;
using Xunit;

namespace FootballLeague.Tests.Services
{
    public class ScoringServiceTests
    {
        private readonly ScoringService scoringService = new ScoringService();

        [Theory]
        [InlineData(0, 0, 0, 0)] // 0 points
        [InlineData(1, 0, 0, 3)] // 3 points
        [InlineData(0, 1, 0, 1)] // 1 point
        [InlineData(2, 3, 1, 9)] // 2 wins * 3 points + 3 draws * 1 = 9 points
        public void CalculatePoints_VariousScenarios_ReturnsExpected(int wins, int draws, int losses, int expected)
        {
            var actual = this.scoringService.CalculatePoints(wins, draws, losses);
            Assert.Equal(expected, actual);
        }
    }
}
