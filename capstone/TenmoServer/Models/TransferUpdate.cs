using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoServer.Models
{
    public class TransferUpdate
    {

        public int User { get; set; }

        public double AmountToSend { get; set; }
        public TransferUpdate()
        {

        }

        public TransferUpdate(int user, double amountToSend)
        {
            this.User = user;
            this.AmountToSend = amountToSend;

        }
    }
}
