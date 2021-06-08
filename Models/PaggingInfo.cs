using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogsWebAplication.Models
{
    public class PaggingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
    }
}
