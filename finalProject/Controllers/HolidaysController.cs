using finalProject.Models;
using finalProject.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace finalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly IRoleRepo roleRepo;
        private readonly IHolidaysRepository holidaysRepository;
        public HolidaysController(IRoleRepo roleRepo,IHolidaysRepository holidaysRepository)
        {
            this.roleRepo = roleRepo;
            this.holidaysRepository = holidaysRepository;
        }
        [HttpGet]
        public List<Holiday> getHolidays()
        {
            return holidaysRepository.GetHolidays();
        }
        
        //getbyid
        [HttpGet("{id:int}")]
        public ActionResult getbyid(int id)
        {
            Holiday holiday = holidaysRepository.GetById(id);
            if (holiday == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(holiday);
            }
        }

        [HttpGet("{name:alpha}")]
        public ActionResult getbyname(string name)
        {
            Holiday holiday = holidaysRepository.GetByName(name);
            if (holiday == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(holiday);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(Holiday holiday)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Holiday"]))
            {
                if (holiday == null)
                {
                    return BadRequest("Invalid holiday object");
                }

                else if (ModelState.IsValid)
                {
                    holidaysRepository.Add(holiday);
                    holidaysRepository.Save();
                    return CreatedAtAction(nameof(getbyid), new { id = holiday.id }, holiday);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }

        //edit
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult Edit(int id, [FromBody] Holiday holiday)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Holiday"]))
            {
                if (holiday == null || holiday.id != id)
                {
                    return BadRequest("Invalid holiday object or mismatched Id");
                }

                holidaysRepository.Edit(id, holiday);
                holidaysRepository.Save();

                return Ok(holiday);
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }

        //delete
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult delete(int id)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Holiday"]))
            {
                Holiday holiday = holidaysRepository.GetById(id);
                if (holiday == null) return NotFound();
                else
                {
                    holidaysRepository.Delete(id);
                    holidaysRepository.Save();
                    //db.holidays.Remove(s);
                    //db.SaveChanges();
                    return Ok(holiday);
                }
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }

    }
}
