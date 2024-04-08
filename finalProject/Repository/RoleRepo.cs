using finalProject.Models;
using System.Security.Claims;

namespace finalProject.Repository
{

    public class RoleRepo:IRoleRepo
    {
        private readonly APIContext db;

        public RoleRepo(APIContext db)
        {
            this.db = db;
        }



        public bool roleAuth(string username,string[] roles)
        {
            string userid = db.Users.Where(e => e.UserName == username).Select(e => e.Id).FirstOrDefault();
            List<string> rolesid = db.UserRoles.Where(e => e.UserId == userid).Select(e => e.RoleId).ToList();
            List<string> dbroles = new List<string>();

            foreach (string item in rolesid)
            {
                string role = db.Roles.Where(e => e.Id == item).Select(e => e.Name).FirstOrDefault();
                dbroles.Add(role);
            }
            foreach (string item in dbroles)
            {
                foreach(string searshrole in roles)
                {
                    if(item == searshrole)
                    {
                        return true;
                    }
                }
            }
            return false;

        }
    }
}
