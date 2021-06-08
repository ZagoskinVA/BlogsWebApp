using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPostgrasql.Models;

namespace BlogsWebAplication.Models
{
    public class BlogsViewModel
    {
        public List<DisplayBlogModel> Blogs { get; set; }
        public PaggingInfo PaggingInfo { get; set; }
    }
}
