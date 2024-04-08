using finalProject.DTO;
using finalProject.Models;
using finalProject.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace finalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRoleRepo roleRepo;
        private readonly APIContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(IRoleRepo roleRepo,APIContext db,UserManager<ApplicationUser> userManager)
        {
            this.roleRepo = roleRepo;
            this.db = db;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetCustomAuthorization()
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin"]))
            {
                List<string> auth = new List<string>();
                auth.Add("Admin");
                List<string> authorization = db.authorizations.Select(a => a.name).Distinct().ToList();
                foreach (var item in authorization)
                {
                    auth.Add(item);
                }
                return Ok(auth);
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddnewAdmin(AdminInfoDTO adminInfo) 
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userrepeat = db.Users.Where(u=>u.UserName == adminInfo.name).FirstOrDefault();
            if (roleRepo.roleAuth(usernameClaim, ["Admin"]))
            {
                if (adminInfo == null)
                {
                    return BadRequest("Missing data");
                }
                if(userrepeat != null)
                {
                    return BadRequest("Repeated UserName");
                }
                if (adminInfo.role == "Admin")
                {
                    try
                    {
                        ApplicationUser user = new ApplicationUser();
                        user.UserName = adminInfo.name;
                        user.PasswordHash = adminInfo.password;
                        IdentityResult result = await userManager.CreateAsync(user, adminInfo.password);

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
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
                        return Ok(new Exception(ex.Message));
                    }
                    return Ok("Success");
                }
                else
                {
                    List<string> roles = db.authorizations.Where(a => a.name == adminInfo.role).Select(a => a.role).ToList();
                    try
                    {
                        ApplicationUser newuser = new ApplicationUser();
                        newuser.UserName = adminInfo.name;
                        newuser.PasswordHash = adminInfo.password;
                        IdentityResult result = await userManager.CreateAsync(newuser, adminInfo.password);

                        if (result.Succeeded)
                        {
                            await userManager.AddToRolesAsync(newuser, roles);
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
                        return Ok(new Exception(ex.Message));
                    }
                    return Ok("Success");

                }
            }
            else
            {
                return Unauthorized("Unauthorized");
            }
        }
    }
}
