using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Election.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Election.Controllers
{
    [Authorize(Roles = Global.ROLE_ADMIN)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public AdminController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_dbContext.Provinces.ToList());
        }


    }
}