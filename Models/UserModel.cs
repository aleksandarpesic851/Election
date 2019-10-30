using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string userid { get; set; }
        public string username { get; set; }
        public string role { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string election_center { get; set; }

    }
}
