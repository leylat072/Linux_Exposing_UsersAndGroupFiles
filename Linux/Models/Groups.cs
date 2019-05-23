using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Linux.Models
{
       public class Groups
    {
            public string Name { get; set; }
            public string Gid { get; set; }
            public List<string> Member { get; set; }
     
   
    }
}
