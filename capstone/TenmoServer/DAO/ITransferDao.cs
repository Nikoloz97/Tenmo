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

        Transfer MakeTransaction(int userId, int receiverId, double amountToSend);
        Transfer UpdateSenderAccount(int userId, double amountToSend);
        Transfer UpdateReceiverAccount(int receiverId, double amountToSend);

    }
}
