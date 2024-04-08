using finalProject.DTO;
using finalProject.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class UniqueLoginPerDayAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var dbContext = (APIContext)validationContext.GetService(typeof(APIContext));


        if (value != null)
        {
            var dayDate = DateOnly.Parse((string)value); // Convert day_date to DateOnly
            var recordEmpDTO = (RecordEmpDTO)validationContext.ObjectInstance;
            var empName = recordEmpDTO.emp_name;
            var empId = dbContext.newEmployees
            .Where(e => e.FullName == empName)
            .Select(e => e.Id)
            .FirstOrDefault();
            var existingLog = dbContext.logs
                .Where(l => l.day == dayDate && l.emp_id == empId)
                .FirstOrDefault();

            if (existingLog != null)
            {
                return new ValidationResult("Employee has already logged in on this day.");
            }
        }

        return ValidationResult.Success;
    }
}
