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
    public class ProvinceController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ProvinceController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            List<ProvinceModel> arrProvince = new List<ProvinceModel>();
            try
            {
                arrProvince = _dbContext.Provinces.ToList();
            }
            catch
            { 
            }
            return View(arrProvince);
        }

        [HttpPost]
        public IActionResult Create(ProvinceModel province)
        {
            try
            {
                _dbContext.Provinces.Add(province);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            return Redirect("/province/index");
        }

        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                ProvinceModel province = _dbContext.Provinces.Where(s => s.id == id).First();
                _dbContext.Provinces.Remove(province);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        public IActionResult Update(ProvinceModel province)
        {
            try
            {
                _dbContext.Provinces.Update(province);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            return Redirect("/province/index");
        }

    }
}