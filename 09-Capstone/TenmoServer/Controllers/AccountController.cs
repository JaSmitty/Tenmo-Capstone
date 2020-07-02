using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDAO accountDAO;

        public AccountController(AccountDAO accountdao)
        {
            accountDAO = accountdao;
        }

        public string Username
        {
            get
            {
                return this.User.Identity.Name;
            }
        }
        public int UserID
        {
            get
            {
                foreach(System.Security.Claims.Claim claim in User.Claims)
                {
                    if (claim.Type == "sub")
                    {
                        int userID = 0;
                        int.TryParse(claim.Value, out userID);
                        return userID;
                    }
                }
                return 0;
            }
        }

        [HttpGet("balance")]
        public ActionResult<decimal> GetBalance()
        {
            return accountDAO.FindBalance(this.UserID);
        }
    }
}
