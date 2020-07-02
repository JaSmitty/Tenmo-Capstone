using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferDAO /*: /*ITransferDAO*/
    {
        public string ConnectionString { get; set; }
        public TransferDAO(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        public int CreateNewTransfer(int accountTo, int accountFrom, int transferType, decimal amount, int transferStatus)
        {
            int transferID;
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("Insert into transfers(transfer_type_id, transfer_status_id, account_from, account_to, amount) Values (@transferType, @transferStatus, @accountFrom, @accountTo, @amount)", conn);
                cmd.Parameters.AddWithValue("@transferType", transferType);
                cmd.Parameters.AddWithValue("@transferStatus", transferStatus);
                cmd.Parameters.AddWithValue("@accountFrom", accountFrom);
                cmd.Parameters.AddWithValue("@accountTo", accountTo);
                cmd.Parameters.AddWithValue("@amount", amount);
                int rowsAffected = cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT @@IDENTITY", conn);
                transferID = Convert.ToInt32(cmd.ExecuteScalar());

                
            }
            return transferID;
        }


        //public Transfer FindTransferByID(int id)
        //{
        //    using (SqlConnection conn = new SqlConnection(this.ConnectionString))
        //    {
        //        conn.Open();

        //        SqlCommand cmd = new SqlCommand("Select * from transfers where transfer_id = @transferID", conn);
        //        cmd.Parameters.AddWithValue("@transferID", id);

        //    }
        //}

        public List<Transfer> ListTransfers(Account account)
        {
            throw new NotImplementedException();
        }


        //private Transfer HelperTransfer(SqlDataReader rdr)
        //{
        //    Transfer transfer = new Transfer()
        //    {
        //        TransferID = Convert.ToInt32(rdr["transfer_id"]),
        //        TransferTypeID = Convert.ToInt32(rdr["transfer_type_id"]),
        //        TransferStatusID = Convert.ToInt32(rdr["transfer_status_id"]),
        //        AccountTo = Convert.ToInt32(rdr["account_to"])
        //    };
        //}


    }


}
