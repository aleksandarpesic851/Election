using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models.ViewModels
{
    public class StateEmployerViewModel
    {
        //User Model
        public int id { get; set; }
        public string userid { get; set; }
        public string username { get; set; }
        public int province { get; set; }
        public int district { get; set; }
    }
}
