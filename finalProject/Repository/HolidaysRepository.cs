using finalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace finalProject.Repository
{
    public class HolidaysRepository : IHolidaysRepository
    {
         APIContext context;

        public HolidaysRepository(APIContext context)
        {
           this.context = context;
        }

        public List<Holiday> GetHolidays()
        {
            return context.holidays.ToList();
        }

        public Holiday GetById(int id)
        {
            return context.holidays.Find(id);
        }

        public Holiday GetByName(string name)
        {
            return context.holidays.FirstOrDefault(n => n.name == name);
        }

        public void Add(Holiday holiday)
        {
             context.Add(holiday);
        }

        public void Edit(int id, Holiday holiday)
        {
            context.Entry(holiday).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //context.Update(holiday);
        }

        public void Delete(int id)
        {
            context.Remove(GetById(id));
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }

}
