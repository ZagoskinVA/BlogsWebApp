using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogsWebAplication.Models
{
    public class DisplayBlogModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public long BlogId { get; set; }
        public int CategoryId { get; set; }
        public string UserName { get; set; }
    }
}
