using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models
{
    public class PartyModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string candidate { get; set; }
        public string logo { get; set; }
    }
}
