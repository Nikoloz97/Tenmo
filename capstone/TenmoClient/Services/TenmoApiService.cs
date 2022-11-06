using RestSharp;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        public readonly string ApiUrl;

        public TenmoApiService(string apiUrl) : base(apiUrl) { }

        // Add methods to call api (controller) here...
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

        public Transfer UpdateSenderAccount(ApiUser user, double amountToSend)
        {
            // Calls for the PUT method in Transfer-controller with this path 
            RestRequest request = new RestRequest($"transfer/balance/send/{user.UserId}");

            // Need this line for PUT or PUSH (otherwise data is lost) 
            request.AddJsonBody(new TransferUpdate(user.UserId, amountToSend));


            IRestResponse<Transfer> response = client.Put<Transfer>(request);


            return response.Data;
        }

        public Transfer UpdateReceiverAccount(int receiver_id, double amountToSend)
        {
            // Calls for the PUT method in Transfer-controller with this path
           RestRequest request = new RestRequest($"transfer/balance/receive/{receiver_id}");

            // Need this line for PUT or POST (otherwise data is lost) 
            // If receiver account stops updating, issue is here. We added a third property in TransferUpdate class
            request.AddJsonBody(new TransferUpdate(receiver_id, amountToSend));

            // Lets save our response in an object
            // Client is a special key-word (not an object) 
           IRestResponse<Transfer> response = client.Put<Transfer>(request);


            return response.Data;
        }

        public Transfer LogTransfer(int user_id, int receiver_id, double amountToTransfer)
        {
            RestRequest request = new RestRequest($"transfer/Log");

            request.AddJsonBody(new TransferUpdate(user_id, amountToTransfer, receiver_id));

          
            IRestResponse<Transfer> response = client.Post<Transfer>(request);


            return response.Data;
        }

        public List<Transfer> DisplaySendingLog(int user_id)
        {
            RestRequest request = new RestRequest($"transfer/Log/Sending/{user_id}");

            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            return response.Data;
        }

        public List<Transfer> DisplayReceveingLog(int user_id)
        {
            RestRequest request = new RestRequest($"transfer/Log/Receiving/{user_id}");

            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            return response.Data;
        }




    }
}
