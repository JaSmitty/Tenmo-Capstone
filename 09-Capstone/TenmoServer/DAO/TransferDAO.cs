using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferDAO : ITransferDAO
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


        public Transfer FindTransferByID(int id)
        {
            Transfer transfer = null;
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("Select * from transfers where transfer_id = @transferID", conn);
                cmd.Parameters.AddWithValue("@transferID", id);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    transfer = HelperTransfer(rdr);
                }
            }
            return transfer;
        }

        public List<Transfer> ListTransfers(int accountid)
        {
            List<Transfer> transfer = new List<Transfer>();
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();
                { 
                SqlCommand cmd = new SqlCommand("select transfer_id, transfer_type_id, transfer_status_id, account_to, account_from, amount from transfers inner join accounts on accounts.account_id = transfers.account_to where account_to = @accountid or account_from = @accountid", conn);
                cmd.Parameters.AddWithValue("@accountid", accountid);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    transfer.Add(HelperTransfer(rdr));
                }
                }
                //{
                //    SqlCommand cmd = new SqlCommand("select transfer_id, transfer_type_id, transfer_status_id, account_to, account_from, amount from transfers inner join accounts on accounts.account_id = transfers.account_from where account_id = @accountid", conn);
                //    cmd.Parameters.AddWithValue("@accountid", accountid);
                //    SqlDataReader rdr = cmd.ExecuteReader();
                //    while (rdr.Read())
                //    {
                //        transfer.Add(HelperTransfer(rdr));
                //    }
                //}
            }
            return transfer;
        }

//        public Transfer FindTransferByID(int transferID)
//        {
//            Transfer transfer = new Transfer();
//            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
//            {
//                conn.Open();
//                {
//                    SqlCommand cmd = new SqlCommand("SELECT * From transfers where transfer_id = @transferid", conn);
//                    cmd.Parameters.AddWithValue("@transferid", transferID);
//                    SqlDataReader rdr = cmd.ExecuteReader();
//                    rdr.Read();
//                    transfer = HelperTransfer(rdr);
//;               }
//            }
//            return transfer;

//        }
        private Transfer HelperTransfer(SqlDataReader rdr)
        {
            Transfer transfer = new Transfer()
            {
                TransferID = Convert.ToInt32(rdr["transfer_id"]),
                TransferTypeID = Convert.ToInt32(rdr["transfer_type_id"]),
                TransferStatusID = Convert.ToInt32(rdr["transfer_status_id"]),
                AccountTo = Convert.ToInt32(rdr["account_to"]),
                AccountFrom = Convert.ToInt32(rdr["account_from"])
            };
            return transfer;
        }


    }


}
