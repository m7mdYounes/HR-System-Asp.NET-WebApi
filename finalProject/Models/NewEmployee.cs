
using System.ComponentModel.DataAnnotations;

namespace finalProject.Models
{
    public class NewEmployee
    {
        [Key]
        public int Id { get; set; }
        public string Address { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]

        public string Number { get; set; }
        [Required]

        public string gender { get; set; }
        [Required]

        public string Nationality { get; set; }
        [Required]

        public DateOnly Birthdate { get; set; }
        [Required]
        
        public string NationalIdNumber { get; set; }
        [Required]


        public DateOnly ContractDate { get; set; }
        [Required]

        public float Salary { get; set; }
        [Required]

        public TimeSpan StartTime { get; set; }
        [Required]

        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; } = true;

        
        public List<Logs> logs { get; set; }
    }
}
