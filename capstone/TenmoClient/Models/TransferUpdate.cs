using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class TransferUpdate
    {

        public int UserID { get; set; }

        public double AmountToSend { get; set; }
        public TransferUpdate()
        {

        }

        public TransferUpdate(int user_id, double amountToSend)
        {
            this.UserID = user_id;
            this.AmountToSend = amountToSend;

        }
    }
}
