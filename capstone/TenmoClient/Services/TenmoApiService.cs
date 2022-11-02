using RestSharp;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoServer.Models;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        public readonly string ApiUrl;

        public TenmoApiService(string apiUrl) : base(apiUrl) { }

        // Add methods to call api here...
        public TenmoServer.Models.Transfer GetAccountBalance()
        {
            RestRequest request = new RestRequest("balance");
            IRestResponse<TenmoServer.Models.Transfer> response = client.Get<TenmoServer.Models.Transfer>(request);

            
            return response.Data;
        }



    }
}
