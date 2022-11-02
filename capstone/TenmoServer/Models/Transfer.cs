using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public double Balance { get; set; }
        // add more stuff lol
    }
}
