using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public double Balance { get; set; }


        public Transfer()
        {
            //must have parameterless constructor to deserialize
            //deserialize = converting string (JSON) -> C# object(s)
        }

        public Transfer(int accountId, int userId, double balance)
        {
            AccountId = accountId;
            UserId = userId;
            Balance = balance;

        }

    }
}
