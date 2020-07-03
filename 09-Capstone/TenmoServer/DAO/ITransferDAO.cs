using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        int CreateNewTransfer(int accountTo, int accountFrom, int transferType, decimal amount, int transferStatus);

        List<Transfer> ListTransfers(int accountid);

        Transfer FindTransferByID(int id);
    }
}
