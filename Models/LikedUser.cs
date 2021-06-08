using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TestPostgrasql.Models;

namespace BlogsWebAplication.Models
{
    public class LikedUser
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("BlogId")]
        public long BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
