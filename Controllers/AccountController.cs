using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Election.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Election
{
    public class AccountController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync();
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Account/Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
        {
            if (LoginUser(model.userName))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.userName),
                    new Claim(ClaimTypes.Role, model.role),
                    new Claim("userId", "10"),
                    new Claim("username", model.userName),
                    new Claim("role", model.role)
                };

                var userIdentity = new ClaimsIdentity(claims, "user");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);

                //Just redirect to our index after logging in. 
                return Redirect(returnUrl);
            }
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        private bool LoginUser(string username)
        {
            //As an example. This method would go to our data store and validate that the combination is correct. 
            //For now just return true. 
            return true;
        }
    }
}
