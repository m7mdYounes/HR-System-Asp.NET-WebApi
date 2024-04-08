using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using finalProject.Models.Validations;
using Microsoft.EntityFrameworkCore;

namespace finalProject.Models
{

    [DifferentHolidayDays]
    public class Settings
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int AdditionalHourRate { get; set; }

        [Required]
        public int LateDeductionRate { get; set; }

        public int HolidayDayOne { get; set; }

        public int HolidayDayTwo { get; set; }
    }
}
