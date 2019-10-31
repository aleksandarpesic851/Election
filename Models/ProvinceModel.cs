using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models
{
    public class ProvinceModel
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "Province")]
        public string name { get; set; }
    }
}
