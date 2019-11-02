using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Election.Controllers
{
    public class CensusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}