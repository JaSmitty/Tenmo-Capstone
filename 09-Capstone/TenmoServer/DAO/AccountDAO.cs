using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountDAO : IAccountDAO
    {
        public string ConnectionString { get; set; }
        public AccountDAO(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public bool DepositMoney(int userID, decimal amount)
        {
            Account account = GetAccount(userID);
            decimal newBalance = account.Balance + amount;
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE accounts set balance = @balance where account_id = @accountID", conn);
                cmd.Parameters.AddWithValue("@balance", newBalance);
                cmd.Parameters.AddWithValue("@accountID", account.AccountID);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public decimal FindBalance(int userID)
        {
            Account account = GetAccount(userID);
            return account.Balance;
        }

        public List<Account> ListAccounts()
        {
            List<Account> accounts = new List<Account>();
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * From accounts", conn);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Account account = HelperAccount(rdr);
                        accounts.Add(account);
                    }

                }

            }
            return accounts;
        }

        public bool WithdrawMoney(int userID, decimal amount)
        {
            Account account = GetAccount(userID);
            decimal newBalance = account.Balance - amount;
            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE accounts set balance = @balance where account_id = @accountID", conn);
                cmd.Parameters.AddWithValue("@balance", newBalance);
                cmd.Parameters.AddWithValue("@accountID", account.AccountID);
                int rowsAffected = cmd.ExecuteNonQuery();
                if(rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        private Account GetAccount(int userID)
        {
            Account account;

            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT account_id, users.user_id, balance, username FROM accounts inner join users on accounts.user_id = users.user_id where user_id = @userID", conn);
                cmd.Parameters.AddWithValue("@userID", userID);
                SqlDataReader rdr = cmd.ExecuteReader();

                account = HelperAccount(rdr);
            }
            return account;
        }

        private Account HelperAccount(SqlDataReader rdr)
        {
            Account account = new Account()
            {
                AccountID = Convert.ToInt32(rdr["account_id"]),
                UserID = Convert.ToInt32(rdr["user_id"]),
                Balance = Convert.ToDecimal(rdr["balance"]),
                Username = Convert.ToString(rdr["username"])
            };

            return account;
        }
    }
}
