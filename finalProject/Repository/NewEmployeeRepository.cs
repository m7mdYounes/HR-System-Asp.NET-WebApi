using System.Globalization;
using System;
using finalProject.DTO;
using finalProject.Models;

namespace finalProject.Repository
{
    public class NewEmployeeRepository : INewEmployeeRepository
    {
        APIContext empContext;
        public NewEmployeeRepository(APIContext _empContext)
        {
            empContext = _empContext;
        }
        public void Add(NewEmployee emp)
        {

            empContext.newEmployees.Add(emp);
            empContext.SaveChanges();
        }

        public void Delete(NewEmployee employeee)
        {
            employeee.IsActive = false;
            empContext.SaveChanges();

        }

        public void Edit(int id, EmployeeDTO updated)
        {

            NewEmployee emp = getByID(id);
            emp.FullName = updated.FullName;
            emp.Address = updated.Address;
            emp.Number = updated.PhoneNumber;
            //emp.Birthdate = DateOnly.ParseExact(updated.dateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //emp.ContractDate = DateOnly.ParseExact(updated.contractDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);


            emp.Birthdate = DateOnly.Parse(updated.dateOfBirth);
            emp.ContractDate = DateOnly.Parse(updated.contractDate);
            emp.Nationality = updated.nationality;
            emp.NationalIdNumber = updated.nationalIdNumber;
            emp.gender = updated.gender;
            emp.StartTime = TimeSpan.Parse(updated.StartTime);
            emp.EndTime = TimeSpan.Parse(updated.endTime);
            empContext.SaveChanges();

        }

        public List<NewEmployee> getallEmp()
        {
            return empContext.newEmployees.Where(e => e.IsActive == true).ToList();

        }

        public NewEmployee getByID(int id)
        {
            return empContext.newEmployees.Find(id);
        }

        public NewEmployee getByUserName(string userName)
        {
            return empContext.newEmployees.Where(i => i.FullName == userName).FirstOrDefault();
        }

    }
}
