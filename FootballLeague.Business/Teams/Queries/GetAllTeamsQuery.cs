using FootballLeague.Business.Teams.Dtos;
using MediatR;
using System.Collections.Generic;

namespace FootballLeague.Business.Teams.Queries
{
    public class GetAllTeamsQuery : IRequest<List<TeamDto>>
    {
    }
}
