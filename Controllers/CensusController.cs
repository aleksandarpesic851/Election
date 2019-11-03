using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Election.API;
using Election.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Election.Controllers
{
    [Authorize(Roles = Global.ROLE_STATEEMPOYER)]
    public class CensusController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CensusController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index(string errorMsg, string userid)
        {
            int district = Convert.ToInt32(User.FindFirst("district").Value);
            VoterApiClient client = new VoterApiClient();
            Message<List<VoterModel>> results = await client.GetAllVoter(district);
            if (results == null || !results.IsSuccess)
            {
                return Ok("Can't read voter data");
            }
            List<VoterModel> arrVoters = (List<VoterModel>)results.Data;
            
            ViewData["districtName"] = _dbContext.Districts.Find(district).name;
            ViewData["errorMsg"] = errorMsg;
            ViewData["userid"] = userid;
            return View(arrVoters);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VoterModel voter)
        {
            int district = Convert.ToInt32(User.FindFirst("district").Value);
            voter.userid = "";
            voter.district = district;
            
            VoterApiClient client = new VoterApiClient();
            Message<string> results = await client.CreateUpdateVoter(voter);
            if (results.IsSuccess)
            {
                return RedirectToAction("Index", new { userid = results.Data } );
            }
            return RedirectToAction("Index", new { errorMsg = "Create new voter failed" } );
        }

        [HttpPost]
        public async Task<IActionResult> Update(VoterModel voter)
        {
            int district = Convert.ToInt32(User.FindFirst("district").Value);
            voter.district = district;

            VoterApiClient client = new VoterApiClient();
            Message<string> results = await client.CreateUpdateVoter(voter);
            if (results.IsSuccess)
            {
                return Redirect("/Census/Index");
            }
            return RedirectToAction("Index", new { errorMsg = "Update failed" } );
        }

        [HttpPost]
        public async Task<bool> Delete(string userid)
        {
            int district = Convert.ToInt32(User.FindFirst("district").Value);
            List<int> Districts = new List<int>();
            Districts.Add(district);

            try
            {
                VoterSearchModel searchModel = new VoterSearchModel()
                {
                    arrDistricts = Districts,
                    userID = userid
                };
                VoterApiClient client = new VoterApiClient();
                Message<int> results = await client.DeleteVoter(searchModel);
                if (results.IsSuccess)
                    return true;
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
        }
    }
}