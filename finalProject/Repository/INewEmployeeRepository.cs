using finalProject.DTO;
using finalProject.Models;

namespace finalProject.Repository
{
    public interface INewEmployeeRepository
    {
        public List<NewEmployee> getallEmp();
        public NewEmployee getByID(int id);
        public void Add(NewEmployee emp);
        public void Edit(int id, EmployeeDTO emp);
        public void Delete(NewEmployee emp);
        public NewEmployee getByUserName(string userName);
    }
}
