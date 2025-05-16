using FootballLeague.Business.Teams.Dtos;
using MediatR;

namespace FootballLeague.Business.Teams.Queries
{
    public class GetTeamByIdQuery : IRequest<TeamDto>
    {
        public GetTeamByIdQuery(int id)
        {
            this.Id = id;
        }

        public int Id { get; }
    }
}
