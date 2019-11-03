using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Election.API;
using Election.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Election
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult Test()
        {
            return BadRequest();

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Account/Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            /*
            UserModel newUser = new UserModel
            {
                username = "test",
                role = "supervisor",
                province = 1,
                district = 2,
                election_center = 3
            };
            _dbContext.Accounts.Add(newUser);
            _dbContext.SaveChanges();
            */
            
            UserModel userModel = GetUser(model);
            List<Claim> claims;
            //if can't find any admin, try to find voter
            if (userModel == null)
            {
                //Get Voter with login id
                VoterSearchModel voterSearch = new VoterSearchModel();
                voterSearch.arrDistricts = new List<int>();
                voterSearch.userID = model.userid;
                List<ElectionDistrictModel> arrDistricts = _dbContext.Districts.ToList();
                foreach (ElectionDistrictModel district in arrDistricts)
                {
                    voterSearch.arrDistricts.Add(district.id);
                }

                VoterApiClient client = new VoterApiClient();
                Message<VoterModel> response = await client.GetVoter(voterSearch);

                if (!response.IsSuccess)
                {
                    ViewData["ErrorMessage"] = "Your login ID isn't correct. Please type your account ID correctly.";
                    return View();
                }

                VoterModel voter = (VoterModel)response.Data;
                //If A election center supervisor didn't checked this user, can't login
                if (voter.vote_state == 0)
                {
                    ViewData["ErrorMessage"] = "You can login after a supervisor check your account";
                    return View();
                }

                //a user already voted, can't login
                if (voter.vote_state > 1)
                {
                    ViewData["ErrorMessage"] = "You already voted";
                    return View();
                }

                //Get election start/end date / time
                ElectionDateModel electionDate = _dbContext.ElectionDates.First();
                DateTime currentDateTime = System.DateTime.Now;
                if (currentDateTime < electionDate.start_time || currentDateTime > electionDate.end_time)
                {
                    ViewData["ErrorMessage"] = "Election is started at " + electionDate.start_time + ". and ended at " + electionDate.end_time;
                    return View();
                }
                claims = new List<Claim>
                {
                    new Claim("username", voter.username == null ? "" : voter.username),
                    new Claim(ClaimTypes.Role, Global.ROLE_VOTER),
                    new Claim("id", "" + voter.id),
                    new Claim("userId", "" + voter.userid),
                    new Claim("nic", "" + voter.nic),
                    new Claim("address", "" + voter.address),
                    new Claim("district", "" + voter.district),
                    new Claim("vote_state", "" + voter.vote_state)
                };
            }
            //if the user is admin or supervisor or employer
            else
            {
                claims = new List<Claim>
                {
                    new Claim("username", userModel.username == null ? "" : userModel.username),
                    new Claim(ClaimTypes.Role, userModel.role),
                    new Claim("userId", "" + userModel.id),
                    new Claim("province", "" + userModel.province),
                    new Claim("district", "" + userModel.district),
                    new Claim("election_center", "" + userModel.election_center)
                };
            }
            var userIdentity = new ClaimsIdentity(claims, "user");

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);

            if (returnUrl == null)
            {
                returnUrl = "/";
            }
            //Just redirect to our index after logging in. 
            return Redirect(returnUrl);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        // get user from login information
        private UserModel GetUser(LoginViewModel model)
        {
            try
            {
                return _dbContext.Accounts.Where(s => s.userid == model.userid).First();

            }
            catch(Exception e)
            {
                return null;
            }
            
        }
    }
}
