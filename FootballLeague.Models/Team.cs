using System.Collections.Generic;

namespace FootballLeague.Models
{
    public class Team
    {
        public Team()
        {
            this.IsRetired = false;
            this.HomeMatches = new List<Match>();
            this.AwayMatches = new List<Match>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsRetired { get; set; }

        public ICollection<Match> HomeMatches { get; set; }

        public ICollection<Match> AwayMatches { get; set; }
    }
}
