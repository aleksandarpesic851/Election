using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models.ViewModels
{
    public class VoteViewModel
    {
        public VoterModel voter { get; set; }
        public List<PartyModel> arrPary { get; set; }
        public string district { get; set; }
        public string province { get; set; }
    }
}
