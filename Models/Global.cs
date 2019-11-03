using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Election.Models
{
    public class Global
    {
        public const string ROLE_ADMIN = "admin";
        public const string ROLE_STATEEMPOYER = "state employer";
        public const string ROLE_SUPERVISOR = "supervisor";
        public const string ROLE_VOTER = "voter";

        public const string API_BASIC_URL = "http://localhost:7200";
        public const string API_CREATE_UPDATE_VOTER = "Voter/CreateUpdateVoter";
        public const string API_GET_RESULT = "Voter/GetResult";
        public const string API_GET_VOTER = "Voter/GetVoter";
        public const string API_GET_ALL_VOTER = "Voter/GetAllVoter";
        public const string API_VOTE = "Voter/Vote";
        public const string API_DELETE_VOTER = "Voter/DeleteVoter";
    }
}
