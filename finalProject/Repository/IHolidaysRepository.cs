using finalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace finalProject.Repository
{
    public interface IHolidaysRepository
    {
        List<Holiday> GetHolidays();
        public Holiday GetById(int id);
        Holiday GetByName(string name);
        public void Add(Holiday holiday);
        public void Edit(int id, Holiday holiday);
        public void Delete(int id);
        public void Save();
    }
}
