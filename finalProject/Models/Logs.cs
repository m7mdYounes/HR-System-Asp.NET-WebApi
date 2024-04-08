using finalProject.Models.Validations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace finalProject.Models
{
    public class Logs
    {
        public int id { get; set; }

        public DateOnly day { get; set; }
        
        public TimeOnly day_arrival { get; set; }
        public TimeOnly day_departure { get; set; }

        [ForeignKey("employee")]
        public int emp_id { get; set; }
        public NewEmployee? employee { get; set; }
    }
}
