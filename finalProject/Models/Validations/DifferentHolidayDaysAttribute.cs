using System.ComponentModel.DataAnnotations;

namespace finalProject.Models.Validations
{
    public class DifferentHolidayDaysAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var settings = (Settings)value;

            if (settings.HolidayDayOne == settings.HolidayDayTwo)
            {
                return new ValidationResult("HolidayDayOne must be different from HolidayDayTwo.");
            }

            return ValidationResult.Success;
        }
    }
}
