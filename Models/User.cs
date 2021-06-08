using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestPostgrasql.Models
{
    public class User : IdentityUser
    {
        public string NickName { get; set; }
        public List<Blog> Blogs { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; }



    }
}