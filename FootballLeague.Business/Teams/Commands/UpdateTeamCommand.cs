using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Business.Teams.Commands
{
    public class UpdateTeamCommand : IRequest<Unit>
    {
        public UpdateTeamCommand(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; }
    }
}
