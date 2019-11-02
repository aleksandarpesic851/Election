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
    public class SupervisorController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public SupervisorController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(string errorMsg)
        {
            int responsableDistrictId = Convert.ToInt32(User.FindFirst("district").Value.ToString());
            
            List<UserModel> arrUsers = new List<UserModel>();
            
            try
            {
                List<ElectionCenterModel> arrElectionCenters = _dbContext.ElectionCenters.Where(e => e.district == responsableDistrictId).ToList();
                List<int> arrCenterIds = new List<int>();
                foreach(ElectionCenterModel element in arrElectionCenters)
                {
                    arrCenterIds.Add(element.id);
                }
                // Get all users who is supervisor in this district
                arrUsers = _dbContext.Accounts.Where(d=> arrCenterIds.Contains(Convert.ToInt32(d.election_center))).ToList();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            //get resposable election centers
            ViewData["electionCenters"] = _dbContext.ElectionCenters.Where(e=>e.district == responsableDistrictId).ToList();
            ViewData["errorMsg"] = errorMsg;
            return View(arrUsers);
        }


        [HttpPost]
        public IActionResult Create(UserModel supervisor)
        {
            
            //if there is an account with this id, send error message.
            if (_dbContext.Accounts.Count(d => d.userid == supervisor.userid) > 0)
            {
                return RedirectToAction("index", new { errorMsg = "There is an account with this id. please try again with other id" } );
            }
            supervisor.role = Global.ROLE_SUPERVISOR;
            _dbContext.Accounts.Add(supervisor);
            _dbContext.SaveChanges();

            return Redirect("/Supervisor/Index");

        }


        [HttpPost]
        public IActionResult Update(UserModel supervisor)
        {

            supervisor.role = Global.ROLE_SUPERVISOR;
            _dbContext.Accounts.Update(supervisor);
            _dbContext.SaveChanges();

            return Redirect("/Supervisor/Index");

        }

        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                UserModel stateEmployer = _dbContext.Accounts.Find(id);
                _dbContext.Accounts.Remove(stateEmployer);
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