using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransactionDao
    {
        Transfer GetAccountBalance(int userId);

        Transfer SendFunds(int userId, int receiverId);

        List<Transfer> GetTransgerLog();

        List<Transfer> GetTransfersDetails(int transferId);




    }
}
