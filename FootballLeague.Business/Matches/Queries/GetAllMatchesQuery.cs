using FootballLeague.Business.Matches.Dtos;
using MediatR;
using System.Collections.Generic;

namespace FootballLeague.Business.Matches.Queries
{
    public class GetAllMatchesQuery : IRequest<List<MatchDto>>
    {
    }
}
