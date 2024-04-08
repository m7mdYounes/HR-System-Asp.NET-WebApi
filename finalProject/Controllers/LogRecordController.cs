using finalProject.DTO;
using finalProject.Models;
using finalProject.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace finalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogRecordController : ControllerBase
    {
        private readonly ILogsRepo logsRepo;
        private readonly APIContext db;
        private readonly IRoleRepo roleRepo;

        public LogRecordController(ILogsRepo logsRepo, APIContext db, IRoleRepo roleRepo)
        {
            this.logsRepo = logsRepo;
            this.db = db;
            this.roleRepo = roleRepo;
        }


        [HttpPost]
        [Authorize]
        public ActionResult AddRecord(RecordEmpDTO recordEmpDTO)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Records"]))
            {
                if (recordEmpDTO == null)
                {
                    return BadRequest();
                }
                else if (ModelState.IsValid)
                {
                    Logs log = logsRepo.convertDTOtoLOG(recordEmpDTO);
                    logsRepo.addrecord(log);

                    return CreatedAtAction("GetEmpNames", log);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }


        [HttpGet("{date:alpha}")]
        [Authorize]
        public ActionResult GetAttendanceByDay(string date)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Records"]))
            {
                return Ok(logsRepo.GetRecordsByDay(date));
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }


        [HttpGet]
        public ActionResult GetEmpNames()
        {
            return Ok(db.newEmployees.Where(e=>e.IsActive == true).ToList());
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public ActionResult deleterecord(int id)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Records"]))
            {
                Logs log = logsRepo.getbyid(id);
                logsRepo.deleterecord(id);
                return Ok(log);
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public ActionResult updaterecord(int id)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Records"]))
            {
                Logs log = logsRepo.getbyid(id);
                logsRepo.updaterecord(id);
                return Ok(log);
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public ActionResult Getbyid(int id)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Records"]))
            {
                return Ok(logsRepo.getbyid(id));
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
            }

        [HttpGet("{Year:int}/{Month:int}")]
        [Authorize]
        public IActionResult GetAllAttendanceInOneMonth(int Year, int Month)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Records"]))
            {
                return Ok(logsRepo.GetAllAttendforMonthYear(Year, Month));
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }

        [HttpGet("{empName:alpha}/{Year:int}/{Month:int}")]
        [Authorize]
        public ActionResult GetAttendanceForOneEmp(string empName, int Year, int Month)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["View"]))
            {
                return Ok(logsRepo.GetAttendforEmpbyname(empName, Year, Month));
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }

    }
}
