using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private ITransferDao transferDao;
        public TransferController(ITransferDao transfer)
        {
            this.transferDao = transfer;
        }
      
        
       [HttpGet("balance/{user_id}")]
      public ActionResult<Transfer> GetBalance(int user_id)
        {
            Transfer transfer = transferDao.GetBalance(user_id);

            return transfer;

        }


        // Do we need this??
        [HttpPost()]
        public ActionResult<Transfer> MakeTransaction(int userId, int receiverId, double balance)
        {
            Transfer transfer = transferDao.MakeTransaction(userId, receiverId, balance);
            return transfer;
        }



        [HttpPut("balance/send/{userId}")]
        public ActionResult<Transfer> UpdateSenderAccount(TransferUpdate transferUpdate)
        {
            Transfer transfer = transferDao.UpdateSenderAccount(transferUpdate.UserID, transferUpdate.AmountToSend);
            return transfer;
        }


        // The "request.AddJsonBody(new TransferUpdate(receiver_id, amountToSend))" from API service gets delivered here...
        [HttpPut("balance/receive/{receiverId}")]
        public ActionResult<Transfer> UpdateReceiverAccount(TransferUpdate transferUpdate)
        {
            Transfer transfer = transferDao.UpdateReceiverAccount(transferUpdate.UserID, transferUpdate.AmountToSend);
            return transfer;
        }




    }
}
