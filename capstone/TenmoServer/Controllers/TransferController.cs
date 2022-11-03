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

     


        
    }
}
