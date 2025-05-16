using FootballLeague.Business.Helpers.Validations;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Business.Matches.Commands
{
    public class CreateMatchCommand : IRequest<int>
    {
        public CreateMatchCommand(int homeTeamId, int awayTeamId, int homeScore, int awayScore, DateTime date)
        {
            this.HomeTeamId = homeTeamId;
            this.AwayTeamId = awayTeamId;
            this.HomeScore = homeScore;
            this.AwayScore = awayScore;
            this.Date = date;
        }

        [Range(1, int.MaxValue, ErrorMessage = "HomeTeamId must be at least 1.")]
        public int HomeTeamId { get; }

        [Range(1, int.MaxValue, ErrorMessage = "AwayTeamId must be at least 1.")]
        public int AwayTeamId { get; }

        [Range(0, int.MaxValue, ErrorMessage = "HomeScore cannot be negative.")]
        public int HomeScore { get; }

        [Range(0, int.MaxValue, ErrorMessage = "AwayScore cannot be negative.")]
        public int AwayScore { get; }

        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(ValidationHelpers), nameof(ValidationHelpers.ValidateDateNotInFuture))]
        public DateTime Date { get; }
    }
}
