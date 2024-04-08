using finalProject.DTO;
using System.ComponentModel.DataAnnotations;

namespace finalProject.Models.Validations
{
    public class ValidateArrivalDepartureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is RecordEmpDTO logs)
            {
                if (TimeOnly.TryParse(logs.attendance_time, out var arrivalTime) && TimeOnly.TryParse(logs.departure_time, out var departureTime))
                {
                    if (arrivalTime >= departureTime)
                    {
                        return new ValidationResult("Departure time must be after arrival time.");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }

}
