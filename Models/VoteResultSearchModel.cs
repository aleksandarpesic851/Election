using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models
{
    public class VoteResultSearchModel
    {
        public List<int> arrDistricts { get; set; }
        public int party { get; set; }
    }
}
