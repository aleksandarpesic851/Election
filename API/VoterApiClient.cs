using Election.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Election.API
{
    public class VoterApiClient : ApiClient
    {
        //Create new Voter
        public async Task<Message<string>> CreateUpdateVoter(VoterModel voter)
        {
            var requestUrl = CreateRequestUri(Global.API_CREATE_UPDATE_VOTER);
            return await PostAsync<string, VoterModel>(requestUrl, voter);
        }

        //Get Election Result in a district
        public async Task<Message<VoteResult>> GetResult(VoteResultSearchModel searchModel)
        {
            var requestUrl = CreateRequestUri(Global.API_GET_RESULT);
            //return await GetAsync<VoteResult>(requestUrl);
            return await PostAsync<VoteResult, VoteResultSearchModel>(requestUrl, searchModel);
        }

        //Search Voter, This is used in login
        public async Task<Message<VoterModel>> GetVoter(VoterSearchModel voterSearch)
        {
            var requestUrl = CreateRequestUri(Global.API_GET_VOTER);
            return await PostAsync<VoterModel, VoterSearchModel>(requestUrl, voterSearch);
        }

        //get all voters in district
        public async Task<Message<List<VoterModel>>> GetAllVoter(int district)
        {
            var requestUrl = CreateRequestUri(Global.API_GET_ALL_VOTER, "district=" + district);

            return await GetAsync<List<VoterModel>>(requestUrl);
        }

        //Vote
        public async Task<Message<int>> Vote(Vote_Model vote)
        {
            var requestUrl = CreateRequestUri(Global.API_VOTE);
            return await PostAsync<int, Vote_Model>(requestUrl, vote);
        }

        //Delete Voter
        public async Task<Message<int>> DeleteVoter(VoterSearchModel voterSearch)
        {
            var requestUrl = CreateRequestUri(Global.API_DELETE_VOTER);
            return await PostAsync<int, VoterSearchModel>(requestUrl, voterSearch);
        }
    }
}
