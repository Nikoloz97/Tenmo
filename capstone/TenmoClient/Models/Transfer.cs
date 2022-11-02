using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public double Balance { get; set; }


    }
}
