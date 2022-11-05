using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
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
        


    }
}
