using finalProject.DTO;
using finalProject.Models;

namespace finalProject.Repository
{
    public interface ILogsRepo
    {
        public void addrecord(Logs logs);
        public void updaterecord(int id);
        public void deleterecord(int id);
        public Logs convertDTOtoLOG(RecordEmpDTO record);
        public List<RecordEmpDTO> GetRecordsByDay(string date);
        public Logs getbyid(int id);
        public RecordEmpDTO convertLOGtoDTO(Logs log);
        public List<RecordEmpDTO> GetAllAttendforMonthYear(int year, int month);
         public List<RecordEmpDTO> GetAttendforEmpbyname(string empName, int year, int month);



    }
}
