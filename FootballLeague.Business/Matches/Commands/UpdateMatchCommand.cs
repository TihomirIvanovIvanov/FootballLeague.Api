using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Business.Matches.Commands
{
    public class UpdateMatchCommand : IRequest<Unit>
    {
        public UpdateMatchCommand(int id, int homeScore, int awayScore)
        {
            this.Id = id;
            this.HomeScore = homeScore;
            this.AwayScore = awayScore;
        }

        [Range(1, int.MaxValue, ErrorMessage = "Id must be at least 1.")]
        public int Id { get; }

        [Range(0, int.MaxValue, ErrorMessage = "HomeScore cannot be negative.")]
        public int HomeScore { get; }

        [Range(0, int.MaxValue, ErrorMessage = "AwayScore cannot be negative.")]
        public int AwayScore { get; }
    }
}
