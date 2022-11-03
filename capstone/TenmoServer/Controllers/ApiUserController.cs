using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiUserController : ControllerBase
    {
        private IUserDao userDao;

        public ApiUserController(IUserDao userDao)
        {
            this.userDao = userDao;
        }

        [HttpGet()]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> users = userDao.GetUsers();
            return users;
        }
    }
    
}
