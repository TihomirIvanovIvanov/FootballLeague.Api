using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Business.Teams.Dtos
{
    public class TeamDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
