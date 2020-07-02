using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    interface ITransferDAO
    {
        int CreateNewTransfer(int accountTo, int accountFrom, int transferType, decimal amount, int transferStatus);

        List<Transfer> ListTransfers(Account account);

        Transfer FindTransferByID(int id);
    }
}
