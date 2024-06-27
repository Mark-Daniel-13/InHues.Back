using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace InHues.Application.Common.Extensions
{
    public static class FluentValidationExtension
    {
        public static List<string> GetErrorMessages(this ValidationResult results) => 
            (results.Errors is not null || results.Errors.Count > 0) ? results.Errors.Select(result=>result.ErrorMessage).ToList(): null;
    }
}
