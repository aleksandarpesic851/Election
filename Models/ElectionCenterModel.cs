using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models
{
    public class ElectionCenterModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? district { get; set; }
        public int? valid { get; set; }
        public int? invalid { get; set; }
    }
}
