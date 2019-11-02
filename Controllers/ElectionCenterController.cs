using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Election.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Election.Controllers
{
    [Authorize(Roles = Global.ROLE_STATEEMPOYER)]
    public class ElectionCenterController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ElectionCenterController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            int responsableDistrictId = Convert.ToInt32(User.FindFirst("district").Value.ToString());
            List<ElectionCenterModel> arrElectionCenters = new List<ElectionCenterModel>();
            try
            {
                // Get all Parties
                arrElectionCenters = _dbContext.ElectionCenters.Where(e=>e.district == responsableDistrictId).ToList();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }

            return View(arrElectionCenters);
        }


        [HttpPost]
        public IActionResult Create(ElectionCenterModel electionCenter)
        {
            int responsableDistrictId = Convert.ToInt32(User.FindFirst("district").Value.ToString());
            electionCenter.district = responsableDistrictId;
            _dbContext.ElectionCenters.Add(electionCenter);
            _dbContext.SaveChanges();

            return Redirect("/ElectionCenter/Index");
        }

        [HttpPost]
        public IActionResult Update(ElectionCenterModel electionCenter)
        {
            int responsableDistrictId = Convert.ToInt32(User.FindFirst("district").Value.ToString());
            electionCenter.district = responsableDistrictId;
            _dbContext.ElectionCenters.Update(electionCenter);
            _dbContext.SaveChanges();

            return Redirect("/ElectionCenter/Index");
        }


        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                ElectionCenterModel electionCenter = _dbContext.ElectionCenters.Find(id);
                _dbContext.ElectionCenters.Remove(electionCenter);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}