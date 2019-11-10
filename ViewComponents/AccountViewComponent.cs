using Election.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Election.ViewComponents
{
    public class AccountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountViewComponent(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> InvokeAsync()
        {
            string result = "";
            string role = UserClaimsPrincipal.FindFirst(ClaimTypes.Role).Value;
            switch(role)
            {
                case Global.ROLE_ADMIN:
                    result = "Admin";
                    break;
                case Global.ROLE_STATEEMPOYER:
                    result = GetResultFromDistrict();
                    break;
                case Global.ROLE_SUPERVISOR:
                    result = GetResultFromCenter();
                    break;
                case Global.ROLE_VOTER:
                    result = GetResultFromDistrict();
                    break;
            }

            return result;
        }

        private string GetResultFromDistrict()
        {
            int district = Convert.ToInt32(UserClaimsPrincipal.FindFirst("district").Value);
            try
            {
                ElectionDistrictModel District = _dbContext.Districts.Find(district);
                ProvinceModel Province = _dbContext.Provinces.Find(District.province);
                return District.name + ", " + Province.name;
            }
            catch { }
            return "";
        }
        private string GetResultFromCenter()
        {
            int center = Convert.ToInt32(UserClaimsPrincipal.FindFirst("election_center").Value);
            try
            {
                ElectionCenterModel Center = _dbContext.ElectionCenters.Find(center);
                ElectionDistrictModel District = _dbContext.Districts.Find(Center.district);
                ProvinceModel Province = _dbContext.Provinces.Find(District.province);
                return Center.name + ", " + District.name + ", " + Province.name;
            }
            catch { }
            return "";
        }
    }
}
