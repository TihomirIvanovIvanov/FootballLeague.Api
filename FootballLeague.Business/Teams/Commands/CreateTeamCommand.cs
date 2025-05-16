using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Business.Teams.Commands
{
    public class CreateTeamCommand : IRequest<int>
    {
        public CreateTeamCommand(string name)
        {
            this.Name = name;
        }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; }
    }
}
