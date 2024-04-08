using finalProject.DTO;
using finalProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace finalProject.Models.Validations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AllowLogAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (APIContext)validationContext.GetService(typeof(APIContext));

            var log = (RecordEmpDTO)value;

            if (IsHoliday(log.day_date, dbContext))
            {
                return new ValidationResult("Logging is not allowed on holidays.");
            }

            if (IsHolidayDayInSettings(log.day_date, dbContext))
            {
                return new ValidationResult("Logging is not allowed on configured holiday days.");
            }

            return ValidationResult.Success;
        }

        private static bool IsHoliday(string date, APIContext dbContext)
        {
            DateTime parsedDate = DateTime.Parse(date);
            DateOnly dateOnly = DateOnly.FromDateTime(parsedDate);

            return dbContext.holidays.Any(h => h.date == dateOnly); 
        }

        private static bool IsHolidayDayInSettings(string date, APIContext dbContext)
        {
            // Convert the date string to DateTime for DayOfWeek calculation
            DateTime dateTime = DateTime.Parse(date);

            // Get the day of the week for the given date
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;

            // Get the configured holiday days from the settings
            Settings settings = dbContext.settings.FirstOrDefault();

            // Check if the day matches either HolidayDayOne or HolidayDayTwo
            return settings != null &&
                   (dayOfWeek == (DayOfWeek)settings.HolidayDayOne ||
                    dayOfWeek == (DayOfWeek)settings.HolidayDayTwo);
        }
    }
}
