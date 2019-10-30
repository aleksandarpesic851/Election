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

        public IActionResult Province()
        {
            return View(_dbContext.Provinces.ToList());
        }

        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                ProvinceModel doctor = _dbContext.Provinces.Where(s => s.id == id).First();
                _dbContext.Provinces.Remove(doctor);
                _dbContext.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }

    }
}