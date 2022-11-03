using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        Transfer GetBalance(int user_id);

        Transfer MakeTransaction(int userID, double amountToSend, double Balance);

    }
}
