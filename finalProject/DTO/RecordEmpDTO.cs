using finalProject.Models;
using finalProject.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace finalProject.DTO

{
    [AllowLog]
    [ValidateArrivalDeparture]
    public class RecordEmpDTO
    {
        public int id { get; set; }
        public string emp_name { get; set; }
        [UniqueLoginPerDay]
        public string day_date { get; set; }

        public string attendance_time { get; set; }
        
        public string departure_time { get; set; }
    }
}
