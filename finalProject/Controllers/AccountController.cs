using finalProject.DTO;
using finalProject.Models;
using finalProject.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace finalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRoleRepo roleRepo;
        private readonly APIContext db;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(IRoleRepo roleRepo,APIContext db, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleRepo = roleRepo;
            this.db = db;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        #region set Roles
        //[HttpGet]
        //public async Task<ActionResult> CreateRole()
        //{
        //    IdentityRole identityRole = new IdentityRole("Admin");
        //    IdentityResult identityResult = await roleManager.CreateAsync(identityRole);

        //    IdentityRole identityRole1 = new IdentityRole("Employee");
        //    IdentityResult identityResult1 = await roleManager.CreateAsync(identityRole1);

        //    IdentityRole identityRole2 = new IdentityRole("Records");
        //    IdentityResult identityResult2 = await roleManager.CreateAsync(identityRole2);

        //    IdentityRole identityRole3 = new IdentityRole("Holiday");
        //    IdentityResult identityResult3 = await roleManager.CreateAsync(identityRole3);

        //    IdentityRole identityRole4 = new IdentityRole("Settings");
        //    IdentityResult identityResult4 = await roleManager.CreateAsync(identityRole4);

        //    IdentityRole identityRole5 = new IdentityRole("Salary");
        //    IdentityResult identityResult5 = await roleManager.CreateAsync(identityRole5);

        //    IdentityRole identityRole6 = new IdentityRole("View");
        //    IdentityResult identityResult6 = await roleManager.CreateAsync(identityRole6);

            
        //    return Ok(db.Roles.ToList());
        //}

        #endregion

        [HttpGet("{username:required}/{password:required}")]
        public async Task<ActionResult> Login(string username, string password)
        {
            ApplicationUser user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Unauthorized("Invalid UserName");
            }
            else
            {
                var passwordHasher = new PasswordHasher<ApplicationUser>();


                PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);


                if (result != PasswordVerificationResult.Success)
                {
                    return Unauthorized("Invalid Password");
                }
                else
                {
                    //generate token

                    #region define claims
                    List<string> rolesid = db.UserRoles.Where(r => r.UserId == user.Id).Select(r => r.RoleId).ToList();
                    List<string> roles = new List<string>();
                    foreach (var role in rolesid)
                    {
                        string newrole = db.Roles.Where(r => r.Id == role).Select(r => r.Name).FirstOrDefault();
                        roles.Add(newrole);
                    }
                    List<Claim> name = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,username),
                    };
                    #endregion



                    #region secret key
                    string key = "welcome to my account hr adminstrator jwtjwtjwtjwt";
                    var secertKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

                    #endregion

                    #region create token

                    var signcer = new SigningCredentials(secertKey, SecurityAlgorithms.HmacSha256);
                    //token object
                    var token = new JwtSecurityToken(
                        claims:name,

                        signingCredentials: signcer,
                        expires: DateTime.Now.AddDays(10));

                    //object => encoded string
                    var stringtoken = new JwtSecurityTokenHandler().WriteToken(token);

                    #endregion
                    TokenRolesDTO tokenRolesDTO = new TokenRolesDTO();
                    tokenRolesDTO.TOKEN = stringtoken;
                    tokenRolesDTO.ROLES = roles;

                    return Ok(tokenRolesDTO);
                }
            }

        }


        [HttpPost]
        [Authorize]
        public ActionResult SetNewRole([FromBody] NewAuthorizationDTO newAuthorization)
        {
            var usernameClaim = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (roleRepo.roleAuth(usernameClaim, ["Admin"]))
            {
                if (newAuthorization == null)
                {
                    return BadRequest("New Role has not authotization");
                }
                string name = db.authorizations.Where(a => a.name == newAuthorization.RoleName).Select(a => a.name).FirstOrDefault();
                if (name != null)
                {
                    return BadRequest("Role name already taken");
                }

                if (ModelState.IsValid)
                {
                    for (int i = 0; i < newAuthorization.Roles.Length; i++)
                    {
                        Authorizations auth = new Authorizations();
                        auth.name = newAuthorization.RoleName;
                        auth.role = newAuthorization.Roles[i];
                        db.authorizations.Add(auth);
                        db.SaveChanges();
                    }
                    return Ok($"{newAuthorization.RoleName} Role has been set");
                }
                else
                {
                    return BadRequest("data is not accurate");
                }


            }
            else
            {
                return Unauthorized("Unauthorized");
            }





        }

    }
}


    

