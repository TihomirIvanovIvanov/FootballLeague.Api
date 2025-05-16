using System;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Business.Helpers.Validations
{
    public static class ValidationHelpers
    {
        public static ValidationResult ValidateDateNotInFuture(DateTime date, ValidationContext context)
        {
            return date <= DateTime.UtcNow
                ? ValidationResult.Success
                : new ValidationResult("Date cannot be in the future.", new[] { context.MemberName });
        }
    }
}
