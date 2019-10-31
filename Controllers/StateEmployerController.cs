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
    public class StateEmployerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public StateEmployerController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(string errorMsg)
        {
            List<UserModel> arrUsers = new List<UserModel>();
            
            try
            {
                // Get all users who is State Emploer
                arrUsers = _dbContext.Accounts.Where(d=>d.district > 0).ToList();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            ViewData["provinces"] = _dbContext.Provinces.ToList();
            ViewData["districts"] = _dbContext.Districts.ToList();
            ViewData["errorMsg"] = errorMsg;
            return View(arrUsers);
        }


        [HttpPost]
        public IActionResult Create(UserModel stateEmpoyer)
        {
            
            //if there is an account with this id, send error message.
            if (_dbContext.Accounts.Count(d => d.userid == stateEmpoyer.userid) > 0)
            {
                return RedirectToAction("index", new { errorMsg = "There is an account with this id. please try again with other id" } );
            }
            _dbContext.Accounts.Add(stateEmpoyer);
            _dbContext.SaveChanges();

            return Redirect("/StateEmployer/Index");

        }


        [HttpPost]
        public IActionResult Update(UserModel stateEmpoyer)
        {

            _dbContext.Accounts.Update(stateEmpoyer);
            _dbContext.SaveChanges();

            return Redirect("/StateEmployer/Index");

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