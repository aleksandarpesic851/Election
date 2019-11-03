using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models.ViewModels
{
    public class PartyResultViewModel
    {
        [Display(Name = "Party")]
        public string name { get; set; }
        [Display(Name = "Candidate")]
        public string candidate { get; set; }
        [Display(Name = "Logo")]
        public string logo { get; set; }
        [Display(Name = "Votes")]
        public int vote { get; set; }
        [Display(Name = "Not Votes")]
        public int not_vote { get; set; }
        [Display(Name = "Registered Voters")]
        public int registerdVoter { get; set; }

        [Display(Name = "Votes(%)")]
        public float percent { get; set; }
    }
}
