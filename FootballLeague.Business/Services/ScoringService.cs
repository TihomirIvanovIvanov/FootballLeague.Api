using FootballLeague.Business.Teams.Dtos;
using FootballLeague.Models;
using System.Linq;

namespace FootballLeague.Business.Services
{
    public class ScoringService : IScoringService
    {
        public TeamRankingDto CalculateTeamRanking(Team team)
        {
            var won = team.HomeMatches.Count(m => m.HomeScore > m.AwayScore)
                    + team.AwayMatches.Count(m => m.AwayScore > m.HomeScore);

            var drawn = team.HomeMatches.Count(m => m.HomeScore == m.AwayScore)
                    + team.AwayMatches.Count(m => m.AwayScore == m.HomeScore);

            var lost = team.HomeMatches.Count(m => m.HomeScore < m.AwayScore)
                    + team.AwayMatches.Count(m => m.AwayScore < m.HomeScore);

            var played = won + drawn + lost;
            var points = CalculatePoints(won, drawn, lost);

            return new TeamRankingDto
            {
                TeamId = team.Id,
                TeamName = team.Name,
                Played = played,
                Won = won,
                Drawn = drawn,
                Lost = lost,
                Points = points
            };
        }

        public int CalculatePoints(int wins, int draws, int losses)
        {
            return wins * 3 + draws;
        }
    }
}
