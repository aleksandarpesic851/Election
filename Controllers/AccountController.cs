using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
            if (userModel == null)
            {
                ViewData["ErrorMessage"] = "Your login ID isn't correct. Please type your account ID correctly.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim("username", userModel.username == null ? "" : userModel.username),
                new Claim(ClaimTypes.Role, userModel.role),
                new Claim("userId", "" + userModel.id),
                new Claim("province", "" + userModel.province),
                new Claim("district", "" + userModel.district),
                new Claim("election_center", "" + userModel.election_center)
            };

            var userIdentity = new ClaimsIdentity(claims, "user");

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);

            if (returnUrl == null)
            {
                switch (userModel.role)
                {
                    case Global.ROLE_ADMIN:
                        return Redirect("/");
                    case Global.ROLE_STATEEMPOYER:
                        return Redirect("/");
                    case Global.ROLE_SUPERVISOR:
                        return Redirect("/");
                    case Global.ROLE_VOTER:
                        return Redirect("/");
                }
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
