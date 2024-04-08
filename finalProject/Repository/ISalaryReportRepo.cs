using finalProject.DTO;

namespace finalProject.Repository
{
    public interface ISalaryReportRepo
    {
        public SalaryReportDTO GetSalaryforEmp(int empId, int year, int month);
        public List<SalaryReportDTO> GetAll(int year, int month);


        public SalaryReportDTO GetbyEmpName(string empName, int year, int month);

    }
}
