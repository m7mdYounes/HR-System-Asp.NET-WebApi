using finalProject.Models;

namespace finalProject.Repository
{
    public interface ISettingsRepository
    {
        List<Settings> GetAllSettings();
        public Settings GetSettingsById(int id);
        public void AddSettings(Settings settings);
        public void UpdateSettings(int id, Settings settings);
        public void DeleteSettings(int id);
        public void Save();
        public void AddOrUpdateSettings(Settings settings);
    }
}
