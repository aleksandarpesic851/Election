using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Election.Models;
using Election.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Election.Controllers
{
    [Authorize(Roles = Global.ROLE_ADMIN)]
    public class ElectionDistrictController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ElectionDistrictController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index(string errorMsg)
        {
            List<ElectionDistrictViewModel> arrDisctrictViewModel = new List<ElectionDistrictViewModel>();
            try
            {
                List<ElectionDistrictModel> arrDistrict = _dbContext.Districts.ToList();
                foreach (ElectionDistrictModel district in arrDistrict)
                {
                    arrDisctrictViewModel.Add(new ElectionDistrictViewModel
                    {
                        id = district.id,
                        name = district.name,
                        provinceId = district.province,
                        provinceName = _dbContext.Provinces.Find(district.province).name
                    });
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            ViewData["provinces"] = _dbContext.Provinces.ToList();
            ViewData["parties"] = _dbContext.Parties.ToList();
            ViewData["errorMsg"] = errorMsg;
            return View(arrDisctrictViewModel);
        }

        [HttpPost]
        public IActionResult Create(ElectionDistrictViewModel electionDistrictViewModel)
        {
/*
            //if there is an account with this id, send error message.
            if (_dbContext.Accounts.Count(d => d.userid == electionDistrictViewModel.stateEmployerUserId) > 0)
            {
                return RedirectToAction("index", new { errorMsg = "There is an account with this id. please try again with other id" } );
            }
*/
            //Create new Election District
            ElectionDistrictModel electionDistrict = new ElectionDistrictModel()
            {
                name = electionDistrictViewModel.name,
                province = electionDistrictViewModel.provinceId
            };
            _dbContext.Districts.Add(electionDistrict);
            _dbContext.SaveChanges();

            return Redirect("/electiondistrict/index");

        }

        [HttpPost]
        public IActionResult Update(ElectionDistrictViewModel electionDistrictViewModel)
        {
            //Update Election District
            ElectionDistrictModel electionDistrict = new ElectionDistrictModel()
            {
                id = electionDistrictViewModel.id,
                name = electionDistrictViewModel.name,
                province = electionDistrictViewModel.provinceId
            };
            _dbContext.Districts.Update(electionDistrict);

            _dbContext.SaveChanges();

            return Redirect("/electiondistrict/index");

        }


        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                ElectionDistrictModel district = _dbContext.Districts.Find(id);
                //Remove all state employers related with this district
                List<UserModel> arrEmployer = _dbContext.Accounts.Where(a => a.district == district.id).ToList();
                foreach(UserModel employer in arrEmployer)
                {
                    _dbContext.Accounts.Remove(employer);
                }
                _dbContext.Districts.Remove(district);

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