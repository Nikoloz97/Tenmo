using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public double Balance { get; set; }
        public double TransferAmount { get; set; }
        public int ReceiverId { get; set; }
        public int TransferId { get; set; }
        public int Transfer_type_id { get; set; }
        public int Transfer_status_id { get; set; }
        public int SenderAccountId { get; set; }
        public int RecieverAccountId { get; set; }



        public Transfer()
        {
            //must have parameterless constructor to deserialize
            //deserialize = converting string (JSON) -> C# object(s)
        }

        public Transfer(int senderAccountId, int userId, double balance)
        {
            SenderAccountId = senderAccountId;
            UserId = userId;
            Balance = balance;

        }

       

        

            







        
    }
}
