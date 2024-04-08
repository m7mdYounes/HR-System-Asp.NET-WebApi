using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace finalProject.Models.Validations
{
    public class OneRawSettingsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            APIContext dbContext = (APIContext)validationContext.GetService(typeof(APIContext));
            Settings settings = (Settings)validationContext.ObjectInstance;

            // Create a new instance of the context with the same connection string or configuration
            var optionsBuilder = new DbContextOptionsBuilder<APIContext>();
            optionsBuilder.UseSqlServer(dbContext.Database.GetDbConnection().ConnectionString);

            using (var readContext = new APIContext(optionsBuilder.Options))
            {
                // Check if the Id exists in the database without tracking
                Settings existingSetting = readContext.settings.AsNoTracking().FirstOrDefault(s => s.Id == settings.Id);

                if (existingSetting != null)
                {
                    // Existing Id found, it's an update
                    return ValidationResult.Success;
                }
            }

            // No existing Id found, it's a new setting
            return new ValidationResult("Settings already exist in the database. You cannot add new settings.");
        }
    }
}
















//protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//{
//    APIContext Context = (APIContext)validationContext.GetService(typeof(APIContext));

//    // Check the number of existing rows in the Settings table
//    var existingRowCount = Context.settings.Count();

//    if (existingRowCount > 0)
//    {
//        return new ValidationResult("Settings already exist in the database. You cannot add new settings.");
//    }

//    return ValidationResult.Success;
//}
//    }
//}