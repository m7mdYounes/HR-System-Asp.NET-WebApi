using finalProject.DTO;
using finalProject.Models;
using finalProject.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
namespace finalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewEmployeeController : ControllerBase
    {

        INewEmployeeRepository IEmp;
        private readonly APIContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRoleRepo roleRepo;

        public NewEmployeeController(INewEmployeeRepository _IEmp,APIContext db ,UserManager<ApplicationUser> userManager, IRoleRepo roleRepo)
        {

            IEmp = _IEmp;
            this.db = db;
            this.userManager = userManager;
            this.roleRepo = roleRepo;
        }
        [HttpGet]
        [Authorize]
        public ActionResult Getall()
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Employee"]))
            {
                List<NewEmployee> Emps = IEmp.getallEmp();
                List<EmployeeReadDTO> Emplist = new List<EmployeeReadDTO>();
                foreach (NewEmployee emp in Emps)
                {
                    EmployeeReadDTO EmpD = new EmployeeReadDTO();
                    EmpD.id = emp.Id;
                    EmpD.FullName = emp.FullName;
                    EmpD.PhoneNumber = emp.Number;
                    EmpD.Address = emp.Address;
                    EmpD.gender = emp.gender;
                    EmpD.nationalIdNumber = emp.NationalIdNumber;
                    EmpD.Nationality = emp.Nationality;
                    EmpD.dateOfBirth = emp.Birthdate.ToString();
                    EmpD.contractDate = emp.ContractDate.ToString();
                    EmpD.Salary = emp.Salary;
                    EmpD.StartTime = emp.StartTime.ToString();
                    EmpD.endTime = emp.EndTime.ToString();
                    Emplist.Add(EmpD);
                }
                if (Emplist == null) return NotFound();
                else
                {
                    return Ok(Emplist);
                }
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }


        [HttpGet("{id:int}")]
        [Authorize]
        public ActionResult<NewEmployee> getbyid(int id)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Employee"]))
            {
                NewEmployee emp = IEmp.getByID(id);
                EmployeeReadDTO Emp = new EmployeeReadDTO()
                {
                    id = emp.Id,
                    FullName = emp.FullName,
                    PhoneNumber = emp.Number,
                    Address = emp.Address,
                    nationalIdNumber = emp.NationalIdNumber,
                    gender = emp.gender,
                    Nationality = emp.Nationality,
                    dateOfBirth = emp.Birthdate.ToString("yyyy-MM-dd"),
                    contractDate = emp.ContractDate.ToString("yyyy-MM-dd"),
                    Salary = emp.Salary,
                    StartTime = emp.StartTime.ToString(),
                    endTime = emp.EndTime.ToString(),
                };

                if (emp == null)
                    return NotFound();
                else
                    return Ok(Emp);
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }
       
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Add(EmployeeDTO emp)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            

            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Employee"]))
            {
                #region validate username and nationalid
                var nationalid = db.newEmployees.Select(e => e.NationalIdNumber).ToList();
                foreach (var item in nationalid)
                {
                    if (item == emp.nationalIdNumber)
                    {
                        return BadRequest("Duplicate NationalIdNumber");
                    }
                }


                var users = db.Users.Select(e => e.UserName).ToList();
                foreach (var item in users)
                {
                    if (item == emp.FullName)
                    {
                        return BadRequest("Duplicate UserName");
                    }
                }


                #endregion
                if (emp.pass == null) return BadRequest("Password is required");
                try
                {
                    NewEmployee emppp = new NewEmployee()
                    {
                        FullName = emp.FullName,
                        Number = emp.PhoneNumber,
                        Address = emp.Address,
                        gender = emp.gender,
                        Nationality = emp.nationality,
                        //Birthdate = DateTime.ParseExact(emp.dateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        //ContractDate = DateTime.ParseExact(emp.contractDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),

                        Birthdate = DateOnly.Parse(emp.dateOfBirth),
                        ContractDate = DateOnly.Parse(emp.contractDate),
                        Salary = emp.Salary,
                        StartTime = TimeSpan.Parse(emp.StartTime),
                        EndTime = TimeSpan.Parse(emp.endTime),
                        NationalIdNumber = emp.nationalIdNumber,

                    };
                    #region set user account

                    try
                    {
                        ApplicationUser user = new ApplicationUser();
                        user.UserName = emp.FullName;
                        user.PasswordHash = emp.pass;
                        IdentityResult result = await userManager.CreateAsync(user, emp.pass);

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "View");

                        }
                        else
                        {
                            foreach (var item in result.Errors)
                            {
                                ModelState.AddModelError("", item.Description);
                            }
                            return BadRequest(result.Errors.ToList());
                        }



                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    #endregion

                    if (emp == null) return BadRequest();
                    if (!ModelState.IsValid)
                        return BadRequest();
                    else
                    {
                        IEmp.Add(emppp);
                        return CreatedAtAction("getbyid", new { id = emppp.Id }, emppp);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return Unauthorized("Unauthorized");
            }

        }
       
        
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult Edit(int id, [FromBody] EmployeeDTO updatedEmployee)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Employee"]))
            {
                var employee = IEmp.getByID(id);
                if (employee == null) return NotFound();
                if (updatedEmployee == null) return BadRequest();
                IEmp.Edit(id, updatedEmployee);
                
                return NoContent();

            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }
       
        
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteEmployee(int id)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin", "Employee"]))
            {
                var employeee = IEmp.getByID(id);
                if (employeee == null)
                {
                    return NotFound();
                }
                IEmp.Delete(employeee);
                return NoContent();
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }


        [HttpGet("{userName:alpha}")]
        [Authorize]
        public ActionResult<NewEmployee> getByUserName(string userName)
        {
            NewEmployee emp = IEmp.getByUserName(userName);
            EmployeeReadDTO Emp = new EmployeeReadDTO()
            {
                id = emp.Id,
                FullName = emp.FullName,
                PhoneNumber = emp.Number,
                Address = emp.Address,
                nationalIdNumber = emp.NationalIdNumber,
                gender = emp.gender,
                Nationality = emp.Nationality,
                dateOfBirth = emp.Birthdate.ToString(),
                contractDate = emp.ContractDate.ToString(),
                Salary = emp.Salary,
                StartTime = emp.StartTime.ToString(),
                endTime = emp.EndTime.ToString(),
            };

            if (emp == null)
                return NotFound();
            else
                return Ok(Emp);
        }


    }
}
