using finalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace finalProject.Repository
{
    public class SettingsRepository:ISettingsRepository
    {
        APIContext context;
        public SettingsRepository(APIContext context)
        {
            this.context = context;
        }
        public List<Settings> GetAllSettings()
        {
            return context.settings.ToList();
        }
        public Settings GetSettingsById(int id)
        {
            return context.settings.Find(id);
        }
        public void AddSettings(Settings settings)
        {
            context.Add(settings);
        }
        public void UpdateSettings(int id, Settings settings)
        {
            var existingSettings = context.settings.Find(id);

            if (existingSettings != null)
            {
                // Detach the existing entity
                context.Entry(existingSettings).State = EntityState.Detached;

                // Update the properties of the existing entity
                existingSettings.AdditionalHourRate = settings.AdditionalHourRate;
                existingSettings.LateDeductionRate = settings.LateDeductionRate;
                existingSettings.HolidayDayOne = settings.HolidayDayOne;
                existingSettings.HolidayDayTwo = settings.HolidayDayTwo;

                // Attach the updated entity and set its state to Modified
                context.Entry(existingSettings).State = EntityState.Modified;
            }
            // Handle the case where the entity with the specified id is not found
            else
            {
                // Handle the error or return appropriate response
            }
        }
        public void DeleteSettings(int id)
        {
            context.Remove(GetSettingsById(id));
            
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public void AddOrUpdateSettings(Settings settings)
        {
            var existingSettings = context.settings.FirstOrDefault(s => s.Id == settings.Id);

            if (existingSettings != null)
            {
                // Update existing settings
                existingSettings.AdditionalHourRate = settings.AdditionalHourRate;
                existingSettings.LateDeductionRate = settings.LateDeductionRate;
                existingSettings.HolidayDayOne = settings.HolidayDayOne;
                existingSettings.HolidayDayTwo = settings.HolidayDayTwo;
            }
            else
            {
                // Add new settings
                context.settings.Add(settings);
            }

            context.SaveChanges();
        }

    }
}
