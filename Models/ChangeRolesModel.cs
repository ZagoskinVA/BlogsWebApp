using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TestPostgrasql.Models
{
    public class ChangeRolesModel
    {
        public string UserId {get; set;}
        
        public string Email {get; set;}

        public List<IdentityRole> AllRoles {get; set;}

        public IList<string> UserRoles {get; set;}
    }
}