using FootballLeague.Business.Matches.Dtos;
using MediatR;
using System.Collections.Generic;

namespace FootballLeague.Business.Matches.Queries
{
    public class GetMatchesByTeamQuery : IRequest<List<MatchDto>>
    {
        public GetMatchesByTeamQuery(int teamId)
        {
            this.TeamId = teamId;
        }

        public int TeamId { get; }
    }
}
