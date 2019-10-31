using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models.ViewModels
{
    public class PartyViewModel
    {
        public int id { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Candidate")]
        public string candidate { get; set; }
        [Display(Name = "Logo Image")]
        public string logo { get; set; }
        public IFormFile logo_file { get; set; }
    }
}
