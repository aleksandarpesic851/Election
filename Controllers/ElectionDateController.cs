using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Election.Models;
using Microsoft.AspNetCore.Mvc;

namespace Election.Controllers
{
    public class ElectionDateController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ElectionDateController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            ElectionDateModel electionDate = new ElectionDateModel();
            try
            {
                electionDate = _dbContext.ElectionDates.First();
            }
            catch (Exception e)
            {

            }
            return View(electionDate);
        }

        public IActionResult Update(ElectionDateModel electionDate)
        {
            _dbContext.ElectionDates.Update(electionDate);
            _dbContext.SaveChanges();
            return Redirect("/ElectionDate/Index");
        }
    }
}