using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    interface IAccountDAO
    {
        decimal FindBalance(int userID);
        bool WithdrawMoney(int userID, decimal amount);
        bool DepositMoney(int userID, decimal amount);
        List<Account> ListAccounts();
    }
}
