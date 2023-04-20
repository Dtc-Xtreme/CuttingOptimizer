using System.ComponentModel.DataAnnotations;

namespace Utilities
{
    public static class Validation
    {
        public static IList<ValidationResult> ValidateModel(object model)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            ValidationContext ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, results, true);
            return results;
        }
    }
}