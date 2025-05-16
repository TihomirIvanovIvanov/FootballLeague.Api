using FootballLeague.Business.Matches.Dtos;
using MediatR;

namespace FootballLeague.Business.Matches.Queries
{
    public class GetMatchByIdQuery : IRequest<MatchDto>
    {
        public GetMatchByIdQuery(int id)
        {
            this.Id = id;
        }

        public int Id { get; }
    }
}
