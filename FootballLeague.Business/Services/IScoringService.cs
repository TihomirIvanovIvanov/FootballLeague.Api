using FootballLeague.Business.Teams.Dtos;
using FootballLeague.Models;

namespace FootballLeague.Business.Services
{
    public interface IScoringService
    {
        TeamRankingDto CalculateTeamRanking(Team team);

        int CalculatePoints(int wins, int draws, int losses);
    }
}
