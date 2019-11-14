using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models.ViewModels
{
    public class ElectionDistrictViewModel
    {
        public int id { get; set; }
        [Display(Name = "District")]
        public string name { get; set; }
        [Display(Name = "Province")]
        public int provinceId { get; set; }

        [Display(Name = "Province")]
        public string provinceName { get; set; }
    }
}
