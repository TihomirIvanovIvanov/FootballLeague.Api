using MediatR;

namespace FootballLeague.Business.Teams.Commands
{
    public class DeleteTeamCommand : IRequest<Unit>
    {
        public DeleteTeamCommand(int id)
        {
            this.Id = id;
        }

        public int Id { get; }
    }
}
