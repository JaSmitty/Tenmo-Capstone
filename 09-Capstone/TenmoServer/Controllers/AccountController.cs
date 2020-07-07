using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountDAO AccountDAO;
        private readonly ITransferDAO TransferDAO;

        public AccountController(IAccountDAO accountdao, ITransferDAO transferDAO)
        {
            AccountDAO = accountdao;
            TransferDAO = transferDAO;
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
            return AccountDAO.FindBalance(this.UserID);
        }


        [HttpGet("users")]
        public ActionResult<List<Account>> Getaccounts()
        {
            return AccountDAO.ListAccounts();
        }

        [HttpPost("sendtransfer")]
        public ActionResult<Transfer> SendTransfer(Transfer transfer1)
        {
            AccountDAO.WithdrawMoney(this.UserID, transfer1.Amount);
            AccountDAO.DepositMoney(transfer1.AccountTo, transfer1.Amount);
            int transferID = TransferDAO.CreateNewTransfer(transfer1.AccountTo, this.UserID, 1, transfer1.Amount, 1);
            Transfer transfer = TransferDAO.FindTransferByID(transferID);
            return transfer;
        }

        [HttpGet("transfers")]
        public ActionResult<List<Transfer>> GetTransfersForAccount()
        {
            return TransferDAO.ListTransfers(this.UserID);
        }

        [HttpGet("transfers/{id}")]
        public ActionResult<Transfer> GetTransferByID(int id)
        {
            Transfer transfer = TransferDAO.FindTransferByID(id);

            return transfer;
        }
    }
}
