using RestSharp;
using System.Collections.Generic;
using TenmoClient.Models;
//using TenmoServer.Models;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        public readonly string ApiUrl;

        public TenmoApiService(string apiUrl) : base(apiUrl) { }

        // Add methods to call api here...
        public Transfer GetAccountBalance(ApiUser user)
        {
            
            RestRequest request = new RestRequest($"transfer/balance/{user.UserId}");
            IRestResponse<Transfer> response = client.Get<Transfer>(request);

            
            return response.Data;
        }



    }
}
