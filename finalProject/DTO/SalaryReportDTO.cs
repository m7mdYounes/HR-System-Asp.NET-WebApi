namespace finalProject.DTO
{
    public class SalaryReportDTO
    {
        public string emp_name { get; set; }
        public float Salary { get; set; }
        public int AttendaceDays { get; set; }
        public int AbsenceDays { get; set; }
        public int AddedHours { get; set; }
        public int lateHours { get; set; }
        public float AddedSalary { get; set; }
        public float SubtractedSalary { get; set; }
        public float OverallSalary { get; set; }


    }
}
