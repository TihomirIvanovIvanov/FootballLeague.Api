using MediatR;

namespace FootballLeague.Business.Matches.Commands
{
    public class DeleteMatchCommand : IRequest<Unit>
    {
        public DeleteMatchCommand(int id)
        {
            this.Id = id;
        }

        public int Id { get; }
    }
}
