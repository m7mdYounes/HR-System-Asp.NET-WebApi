
using finalProject.Models;
using finalProject.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace finalProject.Controllers
{
    [ApiController]
    [Route("api/settings")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IRoleRepo roleRepo;

        public SettingsController(ISettingsRepository settingsRepository,IRoleRepo roleRepo)
        {
            _settingsRepository = settingsRepository;
            this.roleRepo = roleRepo;
        }

        [HttpGet]
        public IActionResult GetAllSettings()
        {
            List<Settings> settings = _settingsRepository.GetAllSettings();
            return Ok(settings);
        }

        [HttpGet("{id}")]
        public IActionResult GetSettingsById(int id)
        {
            Settings settings = _settingsRepository.GetSettingsById(id);
            if (settings == null)
            {
                return NotFound();
            }
            return Ok(settings);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddSettings([FromBody] Settings settings)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Settings"]))
            {
                if (settings == null)
                {
                    return BadRequest("Invalid settings object");
                }

                // Check if there is already a setting in the database
                List<Settings> existingSettings = _settingsRepository.GetAllSettings();

                if (existingSettings.Count > 0)
                {
                    return BadRequest("Settings already exist in the database. You cannot add new settings.");
                }
                else
                {
                    _settingsRepository.AddOrUpdateSettings(settings);
                    _settingsRepository.Save();

                    return Ok(new { message = "Settings added successfully" });
                }
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }


        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateSettings(int id, [FromBody] Settings settings)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Settings"]))
            {
                if (settings == null || settings.Id != id)
                {
                    return BadRequest("Invalid settings object or mismatched Id");
                }

                Settings existingSettings = _settingsRepository.GetSettingsById(id);
                if (existingSettings == null)
                {
                    return NotFound();
                }

                existingSettings.AdditionalHourRate = settings.AdditionalHourRate;
                existingSettings.LateDeductionRate = settings.LateDeductionRate;
                existingSettings.HolidayDayOne = settings.HolidayDayOne;
                existingSettings.HolidayDayTwo = settings.HolidayDayTwo;

                _settingsRepository.UpdateSettings(existingSettings.Id, existingSettings);
                _settingsRepository.Save();

                return Ok("Settings updated successfully");
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSettings(int id)
        {
            Settings existingSettings = _settingsRepository.GetSettingsById(id);
            if (existingSettings == null)
            {
                return NotFound();
            }

            _settingsRepository.DeleteSettings(id);

            return Ok("Settings deleted successfully");
        }
    }
}

//using finalProject.Models;
//using finalProject.Repository;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace finalProject.Controllers
//{

//    [ApiController]
//    [Route("api/settings")]
//    public class SettingsController : ControllerBase
//    {
//        ISettingsRepository settingsRepository;

//        public SettingsController(ISettingsRepository repository)
//        {
//            settingsRepository = repository;
//        }

//        [HttpGet]
//        public IActionResult GetAllSettings()
//        {
//            List<Settings> settings = settingsRepository.GetAllSettings();
//            return Ok(settings);
//        }

//        [HttpGet("{id}")]
//      //  [Authorize(Roles = "Admin , Settings")]
//        public IActionResult GetSettingsById(int id)
//        {
//            Settings settings = settingsRepository.GetSettingsById(id);
//            if (settings == null)
//            {
//                return NotFound();
//            }
//            return Ok(settings);
//        }

//        [HttpPost]
//       // [Authorize(Roles = "Admin , Settings")]
//        public IActionResult AddSettings([FromBody] Settings settings)
//        {
//            Console.WriteLine("Received Request:");
//            Console.WriteLine(settings); // Log the received settings object

//            if (settings == null)
//            {
//                return BadRequest("Invalid holiday object");
//            }
//            else if (ModelState.IsValid)
//            {
//                // Convert holidayDayOne and holidayDayTwo to integers
//                settings.HolidayDayOne = Convert.ToInt32(settings.HolidayDayOne);
//                settings.HolidayDayTwo = Convert.ToInt32(settings.HolidayDayTwo);

//                settingsRepository.AddSettings(settings);
//                settingsRepository.Save();
//                return Ok(new { message = "Settings added successfully" });
//            }
//            else
//            {
//                return BadRequest();
//            }
//        }


//        [HttpPut("{id}")]
//       // [Authorize(Roles = "Admin , Settings")]
//        public IActionResult UpdateSettings(int id, [FromBody] Settings settings)
//        {
//            if (settings == null || settings.Id != id)
//            {
//                return BadRequest("Invalid holiday object or mismatched Id");
//            }

//            Settings existingSettings = settingsRepository.GetSettingsById(id);

//            if (existingSettings == null)
//            {
//                return NotFound();
//            }

//            // Update the properties of the existing entity
//            existingSettings.AdditionalHourRate = settings.AdditionalHourRate;
//            existingSettings.LateDeductionRate = settings.LateDeductionRate;
//            existingSettings.HolidayDayOne = settings.HolidayDayOne;
//            existingSettings.HolidayDayTwo = settings.HolidayDayTwo;

//            // Save the changes
//            settingsRepository.UpdateSettings(id, existingSettings);
//            settingsRepository.Save();

//            return Ok("Settings updated successfully");
//        }


//        [HttpDelete("{id}")]
//       // [Authorize(Roles = "Admin , Settings")]
//        public IActionResult DeleteSettings(int id)
//        {
//            Settings existingSettings = settingsRepository.GetSettingsById(id);
//            if (existingSettings == null)
//            {
//                return NotFound();
//            }

//            settingsRepository.DeleteSettings(id);

//            return Ok("Settings deleted successfully");
//        }
//    }

//}
