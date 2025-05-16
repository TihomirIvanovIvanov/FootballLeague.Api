namespace FootballLeague.Business.Teams.Dtos
{
    public class TeamRankingDto
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public int Played { get; set; }

        public int Won { get; set; }

        public int Drawn { get; set; }

        public int Lost { get; set; }

        public int Points { get; set; }
    }
}
