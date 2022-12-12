using System.ComponentModel.DataAnnotations;

namespace PopularMovieCatalogBackend.Validation
{
    public class FirstLetterUpperCaseAttribution: ValidationAttribute
    {
        
        protected override ValidationResult IsValid( object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            
            var firstLetter = value.ToString()[0].ToString();
            if (firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("First letter should be upperCase");

            }
            //return base.IsValid(value, validationContext);
            return ValidationResult.Success;
        }

    }
}
