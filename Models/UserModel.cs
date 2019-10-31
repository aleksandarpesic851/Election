using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string userid { get; set; }
        [Display(Name = "Name")]
        public string username { get; set; }
        public string role { get; set; }
        [Display(Name = "Province")]
        public int? province { get; set; }
        [Display(Name = "Dostrict")]
        public int? district { get; set; }
        [Display(Name = "Election Center")]
        public int? election_center { get; set; }

    }
}
