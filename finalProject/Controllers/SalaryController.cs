using finalProject.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace finalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryReportRepo salaryRepo;
        private readonly INewEmployeeRepository newEmployeeRepo;
        private readonly IRoleRepo roleRepo;

        public SalaryController(ISalaryReportRepo salaryRepo,INewEmployeeRepository newEmployeeRepo,IRoleRepo roleRepo )
        {
            this.salaryRepo = salaryRepo;
            this.newEmployeeRepo = newEmployeeRepo;
            this.roleRepo = roleRepo;
        }


        [HttpGet]
        public IActionResult GetEmployee() 
        {
            return Ok(newEmployeeRepo.getallEmp());
        }

        [HttpGet("{empId:int}/{Year:int}/{Month:int}")]
        [Authorize]
        public IActionResult GetSales(int empId , int Year,int Month)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Salary"]))
            {
                return Ok(salaryRepo.GetSalaryforEmp(empId, Year, Month));
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }

        [HttpGet("{Year:int}/{Month:int}")]
        [Authorize]
        public IActionResult GetAllsalsries(int Year, int Month)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Salary"]))
            {
                return Ok(salaryRepo.GetAll(Year, Month));
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }

        [HttpGet("{empName:alpha}/{Year:int}/{Month:int}")]
        [Authorize]
        public ActionResult GetSalaryForOneEmp(string empName, int Year, int Month)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["View"]))
            {
                return Ok(salaryRepo.GetbyEmpName(empName, Year, Month));
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }
    }
}
