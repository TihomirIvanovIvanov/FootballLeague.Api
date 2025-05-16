using FootballLeague.Business.Helpers.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Business.Matches.Dtos
{
    public class MatchDto
    {
        [Required]
        public int MatchId { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        [Range(0, int.MaxValue)]
        public int HomeScore { get; set; }

        [Range(0, int.MaxValue)]
        public int AwayScore { get; set; }

        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(ValidationHelpers), nameof(ValidationHelpers.ValidateDateNotInFuture))]
        public DateTime Date { get; set; }
    }
}
