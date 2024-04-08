using System.ComponentModel.DataAnnotations;

namespace finalProject.Models.Validations
{
    public class UniqueHolidayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Holiday holiday = (Holiday)value;
            var context = (APIContext)validationContext.GetService(typeof(APIContext));


            // Check if there is any other holiday with the same name and date
            if (context.holidays.Any(h => h.id != holiday.id && (h.name == holiday.name || h.date == holiday.date)))
            {
                return new ValidationResult("A holiday with the same name or date already exists.");
            }

            return ValidationResult.Success;
        }
    }
}