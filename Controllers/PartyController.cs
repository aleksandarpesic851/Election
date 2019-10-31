using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Election.Models;
using Election.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Election.Controllers
{
    [Authorize(Roles = Global.ROLE_ADMIN)]
    public class PartyController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public PartyController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(string errorMsg)
        {
            List<PartyModel> arrParties = new List<PartyModel>();

            try
            {
                // Get all Parties
                arrParties = _dbContext.Parties.ToList();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            ViewData["errorMsg"] = errorMsg;

            return View(arrParties);
        }


        [HttpPost]
        public async Task<IActionResult> Create(PartyViewModel party)
        {
            string filePath = "";
            string fullPath = "";

            //Save Logo file in wwwroot/party_logo folder
            if (party.logo_file.Length > 0)
            {
                filePath = "/party_logo/" + Path.GetRandomFileName() + party.logo_file.FileName;
                fullPath = Path.GetFullPath("./wwwroot") + filePath;

                using (var stream = System.IO.File.Create(fullPath))
                {
                    await party.logo_file.CopyToAsync(stream);
                }
            }

            //Save party information on db
            PartyModel newParty = new PartyModel()
            {
                name = party.name,
                candidate = party.candidate,
                logo = filePath
            };
            
            _dbContext.Parties.Add(newParty);
            _dbContext.SaveChanges();

            return Redirect("/Party/Index");

        }

        [HttpPost]
        public async Task<IActionResult> Update(PartyViewModel party)
        {
            string filePath = "";
            string fullPath = "";
            //Save Logo file in wwwroot/party_logo folder
            if ( party.logo_file != null && party.logo_file.Length > 0)
            {
                filePath = "/party_logo/" + Path.GetRandomFileName() + party.logo_file.FileName;
                fullPath = Path.GetFullPath("./wwwroot") + filePath;

                using (var stream = System.IO.File.Create(fullPath))
                {
                    await party.logo_file.CopyToAsync(stream);
                }
            }

            //Save party information on db
            PartyModel newParty = new PartyModel()
            {
                id = party.id,
                name = party.name,
                candidate = party.candidate,
                logo = String.IsNullOrEmpty(filePath) ? party.logo : filePath
            };

            _dbContext.Parties.Update(newParty);
            _dbContext.SaveChanges();

            return Redirect("/Party/Index");

        }

        [HttpPost]
        public bool Delete(int id)
        {
            try
            {
                PartyModel party = _dbContext.Parties.Find(id);
                _dbContext.Parties.Remove(party);
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