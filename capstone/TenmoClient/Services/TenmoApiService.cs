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

        public List<ApiUser> GetUser()
        {
            RestRequest request = new RestRequest($"apiuser");
            IRestResponse<List<ApiUser>> response = client.Get<List<ApiUser>>(request);


            return response.Data;
        }

        //public Transfer MakeTransaction(ApiUser user, int receiverId, double amountToSend)
        //{
        //    RestRequest request = new RestRequest($"transfer");
        //    request.AddJsonBody(user);
        //    request.AddJsonBody(receiverId);
        //    request.AddJsonBody(amountToSend);
        //    IRestResponse<Transfer> response = client.Post<Transfer>(request);


        //    return response.Data;
        //}

        //public Transfer UpdateSenderAccount(ApiUser user, double amountToSend)
        //{
        //    RestRequest request = new RestRequest($"transfer/{user.UserId}");
        //    request.AddJsonBody(user);
        //    request.AddJsonBody(amountToSend);
        //    IRestResponse<Transfer> response = client.Put<Transfer>(request);


        //    return response.Data;
        //}

        //public Transfer UpdateReceiverAccount(int receiverId, double amountToSend)
        //{
        //    RestRequest request = new RestRequest($"transfer/{receiverId}");
        //    request.AddJsonBody(receiverId);
        //    request.AddJsonBody(amountToSend);
        //    IRestResponse<Transfer> response = client.Put<Transfer>(request);


        //    return response.Data;
        //}


    }
}
