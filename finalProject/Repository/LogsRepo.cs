
using finalProject.DTO;
using finalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace finalProject.Repository
{
    public class LogsRepo : ILogsRepo
    {
        private readonly APIContext db;

        public LogsRepo(APIContext db)
        {
            this.db = db;
        }
        public Logs convertDTOtoLOG(RecordEmpDTO record)
        {
            int empid = db.newEmployees.Where(e => e.FullName == record.emp_name).Select(e => e.Id).SingleOrDefault();
            Logs log = new Logs();
            log.emp_id = empid;
            log.day = DateOnly.Parse(record.day_date);

            TimeOnly correctarriv = TimeOnly.FromTimeSpan(db.newEmployees.Where(e => e.Id == empid).Select(e => e.StartTime).SingleOrDefault());
            TimeOnly correctdismiss = TimeOnly.FromTimeSpan(db.newEmployees.Where(e => e.Id == empid).Select(e => e.EndTime).SingleOrDefault());
            TimeOnly day_arrival = TimeOnly.Parse(record.attendance_time);
            if (day_arrival.Hour < correctarriv.Hour)
            {
                log.day_arrival = correctarriv;
            }
            else
            {
                log.day_arrival = day_arrival;
            }
            log.day_departure = TimeOnly.Parse(record.departure_time);
            

            return log;

        }
        public RecordEmpDTO convertLOGtoDTO(Logs log)
        {
            RecordEmpDTO recordEmpDTO = new RecordEmpDTO();
            recordEmpDTO.emp_name = log.employee.FullName;
            recordEmpDTO.day_date = log.day.ToString();
            recordEmpDTO.departure_time = log.day_departure.ToString();
            recordEmpDTO.attendance_time = log.day_arrival.ToString();
            return recordEmpDTO;
        }
        public void addrecord(Logs logs)
        {
            db.logs.Add(logs);
            db.SaveChanges();
        }

        public List<RecordEmpDTO> GetRecordsByDay(string date)
        {

            List<Logs> logs = db.logs.Include(e => e.employee).ToList();
            List<RecordEmpDTO> records = new List<RecordEmpDTO>();
            foreach (Logs item in logs)
            {
                RecordEmpDTO record = new RecordEmpDTO();
                record.id = item.id;
                record.emp_name = item.employee.FullName;
                record.day_date = item.day.ToString();
                record.departure_time = item.day_departure.ToString();
                record.attendance_time = item.day_arrival.ToString();

                records.Add(record);
            }
            return records;
        }

        public void updaterecord(int id)
        {
            Logs log = db.logs.Find(id);
            db.Entry<Logs>(log).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void deleterecord(int id)
        {
            Logs log = db.logs.Find(id);
            db.logs.Remove(log);
            db.SaveChanges();
        }

        public Logs getbyid(int id)
        {
            return db.logs.Find(id);
        }

        public List<RecordEmpDTO> GetAttendforEmp(int empId, int year, int month)
        {
            List<RecordEmpDTO> records = new List<RecordEmpDTO>();

            NewEmployee emp = db.newEmployees.FirstOrDefault(e => e.Id == empId &&  e.IsActive == true);
            if (emp != null)
            {
                List<Logs> logs = db.logs.Where(e => e.day.Year == year && e.day.Month == month && e.emp_id == empId).ToList();

                foreach (Logs log in logs)
                {
                    RecordEmpDTO record = new RecordEmpDTO();
                    record.id = emp.Id;
                    record.emp_name = emp.FullName;
                    record.day_date = log.day.ToString("yyyy-MM-dd");
                    record.attendance_time = log.day_arrival.ToString("HH:mm:ss");
                    record.departure_time = log.day_departure.ToString("HH:mm:ss");

                    records.Add(record);
                }
            }

            return records;
        }

        public List<RecordEmpDTO> GetAttendforEmpbyname(string empName, int year, int month)
        {
            var empId = db.newEmployees.Where(e => e.FullName == empName).Select(e => e.Id).FirstOrDefault();
            return GetAttendforEmp(empId, year, month);
        }

        public List<RecordEmpDTO> GetAllAttendforMonthYear(int year, int month)
        {
            List<RecordEmpDTO> allRecords = new List<RecordEmpDTO>();

            List<NewEmployee> employees = db.newEmployees.Where(e=>e.IsActive == true).ToList();

            foreach (var emp in employees)
            {
                List<RecordEmpDTO> empRecords = GetAttendforEmp(emp.Id, year, month);
                allRecords.AddRange(empRecords);
            }

            return allRecords;
        }
    }
}
