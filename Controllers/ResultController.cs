﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Election.API;
using Election.Models;
using Election.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Election.Controllers
{
    public class ResultController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ResultController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole(Global.ROLE_ADMIN))
                return RedirectToAction("Admin");

            if (User.IsInRole(Global.ROLE_STATEEMPOYER))
                return RedirectToAction("StateEmployer");

            if (User.IsInRole(Global.ROLE_SUPERVISOR))
                return RedirectToAction("Supervisor");

            if (User.IsInRole(Global.ROLE_VOTER))
                return RedirectToAction("Voter");

            return Ok();
        }

        [Authorize(Roles = Global.ROLE_ADMIN)]
        public async Task<IActionResult> Admin()
        {
            List<PartyModel> arrParties = _dbContext.Parties.ToList();
            List<ElectionDistrictModel> arrDistricts = _dbContext.Districts.ToList();
            List<PartyResultViewModel> arrResults = new List<PartyResultViewModel>();
            VoterApiClient client = new VoterApiClient();

            foreach (PartyModel party in arrParties)
            {
                PartyResultViewModel partyResult = new PartyResultViewModel();
                partyResult.name = party.name;
                partyResult.logo = party.logo;
                partyResult.candidate = party.candidate;

                foreach (ElectionDistrictModel district in arrDistricts)
                {
                    if (district.party != party.id)
                        continue;
                    //get district result
                    Message<VoteResult> response = await client.GetResult(district.id);

                    if (response.IsSuccess)
                    {
                        VoteResult results = (VoteResult)response.Data;
                        partyResult.registerdVoter += results.registerdVoter;
                        partyResult.vote += results.vote;
                        partyResult.not_vote += results.not_vote;
                    }
                }
                //calculate percent
                if (partyResult.registerdVoter != 0)
                    partyResult.percent = partyResult.vote * 100 / (float)partyResult.registerdVoter;

                arrResults.Add(partyResult);
            }

            return View(arrResults);
        }

        [Authorize(Roles = Global.ROLE_STATEEMPOYER)]
        public async Task<IActionResult> StateEmployer()
        {
            int district = Convert.ToInt32(User.FindFirst("district").Value);
            VoterApiClient client = new VoterApiClient();
            Message<VoteResult> response = await client.GetResult(district);

            VoteResult results = new VoteResult();
            if (response.IsSuccess)
            {
                results = (VoteResult)response.Data;
            }
            ViewData["districtName"] = _dbContext.Districts.Find(district).name;

            return View(results);
        }

        [Authorize(Roles = Global.ROLE_SUPERVISOR)]
        public async Task<IActionResult> Supervisor()
        {
            int centerId = Convert.ToInt32(User.FindFirst("election_center").Value);
            ElectionCenterModel electionCenter = _dbContext.ElectionCenters.Find(centerId);
            ElectionDistrictModel electionDistrict = _dbContext.Districts.Find(electionCenter.district);

            VoterApiClient client = new VoterApiClient();
            List<VoterModel> arrVoters = new List<VoterModel>();
            Message<List<VoterModel>> response = await client.GetAllVoter(electionDistrict.id);
            if (response.IsSuccess)
            {
                arrVoters = response.Data;
            }
            ViewData["center"] = electionCenter.name;
            ViewData["district"] = electionDistrict.name;

            return View(arrVoters);
        }

        public async Task<bool> VoterCheckin([FromBody]Vote_Model voteModel)
        {
            VoterApiClient client = new VoterApiClient();
            Message<int> response = await client.Vote(voteModel);

            return response.IsSuccess;
        }

        [Authorize(Roles = Global.ROLE_VOTER)]
        public IActionResult Voter()
        {
            VoterModel voter = new VoterModel();
            voter.username = User.FindFirst("username").Value;
            voter.userid = User.FindFirst("userid").Value;
            voter.nic = User.FindFirst("nic").Value;
            voter.address = User.FindFirst("address").Value;
            voter.district = Convert.ToInt32(User.FindFirst("district").Value);

            ElectionDistrictModel electionDistrict = _dbContext.Districts.Find(voter.district);
            ProvinceModel province = _dbContext.Provinces.Find(electionDistrict.province);

            ViewData["district"] = electionDistrict.name;
            ViewData["province"] = province.name;

            PartyModel party = _dbContext.Parties.Find(electionDistrict.party);
            ViewData["partyName"] = party.name;
            ViewData["partyLogo"] = party.logo;
            ViewData["partyCandidate"] = party.candidate;

            return View(voter);
        }
        //Vote
        public async Task<bool> Vote(int state)
        {
            Vote_Model voteModel = new Vote_Model();
            voteModel.userid = User.FindFirst("userid").Value;
            voteModel.district = Convert.ToInt32(User.FindFirst("district").Value);
            voteModel.state = state;

            VoterApiClient client = new VoterApiClient();
            Message<int> response = await client.Vote(voteModel);

            return response.IsSuccess;
        }
    }
}