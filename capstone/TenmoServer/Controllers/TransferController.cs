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


        [HttpGet("Log/Sending/{user_id}")]
        public ActionResult<List<Transfer>> DisplaySendingLog (int user_id)
        {
            List<Transfer> transferList = transferDao.DisplaySendingLog(user_id);

            return transferList;

        }

        [HttpGet("Log/FullSending/{user_id}/{transfer_id}")]
        public ActionResult<Transfer> DisplayFullSendingLog(int user_id, int transfer_id)
        {
            Transfer transfer = transferDao.DisplayFullSendingLog(user_id, transfer_id);

            return transfer;

        }


        [HttpGet("Log/Receiving/{user_id}")]
        public ActionResult<List<Transfer>> DisplayReceivingLog (int user_id, int transfer_id)
        {
            List<Transfer> transferList = transferDao.DisplayReceivingLog(user_id);

            return transferList;

        }

        [HttpGet("Log/FullReceiving/{user_id}/{transfer_id}")]
        public ActionResult<Transfer> DisplayFullReceivingLog(int user_id, int transfer_id)
        {
            Transfer transfer = transferDao.DisplayFullReceivingLog(user_id, transfer_id);

            return transfer;

        }


        [HttpPost("Log/test/{user_id}/{reciever_id}/{amountToSend}")]
        public ActionResult<Transfer> LogTransfer(int user_id, int reciever_id, int amountToSend)
        {
            Transfer transfer = transferDao.LogTransfer(user_id, reciever_id, amountToSend);
            return transfer;
        }




        [HttpPut("balance/send/{userId}/{amountToSend}")]
        public ActionResult<Transfer> UpdateSenderAccount(int userId, double amountToSend)
        {
            Transfer transfer = transferDao.UpdateSenderAccount(userId, amountToSend);
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
