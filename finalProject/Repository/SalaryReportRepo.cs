using finalProject.DTO;
using finalProject.Models;

namespace finalProject.Repository
{
    public class SalaryReportRepo : ISalaryReportRepo
    {
        private readonly APIContext db;

        public SalaryReportRepo(APIContext db)
        {
            this.db = db;
        }
        public SalaryReportDTO GetSalaryforEmp(int empId, int year, int month)
        {

            NewEmployee emp = db.newEmployees.Where(e => e.Id == empId && e.IsActive == true).SingleOrDefault();

            TimeOnly emp_arrival = TimeOnly.Parse($"{emp.StartTime}");
            TimeOnly emp_dismissal = TimeOnly.Parse($"{emp.EndTime}");

            #region num of days & hoidays
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int OfficialHolidays = this.CountofHolidays(year, month, db.holidays.Select(h => h.date).ToList());
            int FirstWeekend = this.CountDayOfWeekInMonth(year, month, db.settings.Select(s => s.HolidayDayOne).SingleOrDefault());
            int SecondWeekend = this.CountDayOfWeekInMonth(year, month, db.settings.Select(s => s.HolidayDayTwo).SingleOrDefault());
            int Requiredattendance = daysInMonth - (OfficialHolidays + FirstWeekend + SecondWeekend);
            #endregion


            #region hours late and overtime
            List<Logs> logs = db.logs.Where(e => e.day.Year == year && e.day.Month == month && e.emp_id == empId).ToList();
            int addedhours = 0;
            int subtractedhours = 0;
            
            
            
            foreach (Logs item in logs)
            {
                if(item.day_departure.Hour < emp_dismissal.Hour)
                {
                    subtractedhours += (this.SubtractTimes(emp_dismissal, item.day_departure));
                }
                else
                {
                    addedhours += (this.SubtractTimes(item.day_departure, emp_dismissal));
                }


                subtractedhours += (this.SubtractTimes(item.day_arrival, emp_arrival));
            }





            #endregion


            #region salary
            float dayRate = this.RateOfSalaryPerDay(emp.Salary, Requiredattendance);
            float hourRate = this.RateofSalaryPerHour(emp.Salary, Requiredattendance, emp_dismissal, emp_arrival);

            float addedsalary = addedhours * hourRate * (db.settings.Select(s => s.AdditionalHourRate).FirstOrDefault());
            addedsalary = (float)Math.Round(addedsalary);
            float subsalary = subtractedhours * hourRate * (db.settings.Select(s => s.LateDeductionRate).FirstOrDefault());
            subsalary = (float)Math.Round(subsalary);

            float Salary = ((dayRate * logs.Count()) + addedsalary - subsalary);
            Salary = (float)Math.Round(Salary);

            #endregion



            #region return DTO
            SalaryReportDTO reportDTO = new SalaryReportDTO()
            {
                emp_name = emp.FullName,
                Salary = emp.Salary,
                OverallSalary = Salary,
                AttendaceDays = logs.Count(),
                AbsenceDays = Requiredattendance - logs.Count(),
                AddedHours = addedhours,
                lateHours = subtractedhours,
                AddedSalary = addedsalary,
                SubtractedSalary = subsalary
            };
            return reportDTO;
            #endregion


        }

        private int CountDayOfWeekInMonth(int year, int month, int weekend)
        {
            DayOfWeek dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), $"{weekend}");
            DateTime startDate = new DateTime(year, month, 1);
            int days = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            int weekDayCount = 0;
            for (int day = 0; day < days; ++day)
            {
                weekDayCount += startDate.AddDays(day).DayOfWeek == dayOfWeek ? 1 : 0;
            }
            return weekDayCount;
        }

        private int CountofHolidays(int year, int month, List<DateOnly> dates)
        {
            int count = 0;
            int firstWeekend = db.settings.Select(s => s.HolidayDayTwo).FirstOrDefault();
            int secondWeekend = db.settings.Select(s => s.HolidayDayOne).FirstOrDefault();

            foreach (DateOnly date in dates)
            {
                if (date.Year == year && date.Month == month)
                {
                    count++;

                    if (date.DayOfWeek == (DayOfWeek)firstWeekend || date.DayOfWeek == (DayOfWeek)secondWeekend)
                    {
                        count--;
                    }
                }
            }

            return count;

        }

        public SalaryReportDTO GetbyEmpName(string empName, int year, int month)
        {
            var empId = db.newEmployees.Where(e => e.FullName == empName).Select(e => e.Id).FirstOrDefault();
            return GetSalaryforEmp(empId, year, month);
        }

        private float RateOfSalaryPerDay(float Salary, int CountofDays)
        {
            return Salary / (float)CountofDays;
        }

        private float RateofSalaryPerHour(float Salary, int CountofDays, TimeOnly Dissmisal, TimeOnly Arrival)
        {
            return Salary / ((float)CountofDays * this.SubtractTimes(Dissmisal, Arrival));
        }

        private int SubtractTimes(TimeOnly t1, TimeOnly t2)
        {
            int HourT1 = 0;
            if (t1.Minute != 0)
            {
                HourT1 = t1.Hour + 1;
            }
            else
            {
                HourT1 = t1.Hour;
            }
            int HourT2 = 0;
            if (t2.Minute != 0)
            {
                HourT2 = t2.Hour + 1;
            }
            else
            {
                HourT2 = t2.Hour;
            }
            return HourT1 - HourT2;
        }

        public List<SalaryReportDTO> GetAll(int year, int month)
        {
            List<SalaryReportDTO> salaryReportDTOs = new List<SalaryReportDTO>();
            var empId = db.newEmployees.Where(e=>e.IsActive == true).Select(e => e.Id ).ToList();
            foreach (var item in empId)
            {
                SalaryReportDTO salaryReportDTO = GetSalaryforEmp(item, year, month);
                salaryReportDTOs.Add(salaryReportDTO);
            }
            return salaryReportDTOs;

        }


    }
}
