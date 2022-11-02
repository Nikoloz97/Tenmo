using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using System.Data.SqlClient;

namespace TenmoServer.DAO
{
    public class TransactionsDao : ITransactionDao
    {

        private readonly string connectString;

        private static List<Transfer> Transfers { get; set; }

        public TransactionsDao(string dbConnectionString)
        {
            connectString = dbConnectionString;
        }


        public Transfer GetAccountBalance(int userId)
        {
            Transfer accountBalance = null;

            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select balance from account where user_id = @user_id", conn);
                cmd.Parameters.AddWithValue("@user_id", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    accountBalance = CreateTransferFromReader(reader);
                }
            }
            return null;

        }

        public List<Transfer> GetTransfersDetails(int transferId)
        {
            throw new NotImplementedException();
        }

        public List<Transfer> GetTransgerLog()
        {
            throw new NotImplementedException();
        }

        public Transfer SendFunds(int userId, int receiverId)
        {
            throw new NotImplementedException();
        }


        private Transfer CreateTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.AccountId = Convert.ToInt32(reader["account_id"]);
            transfer.Balance = Convert.ToInt32(reader["balance"]);
            transfer.UserId = Convert.ToInt32(reader["user_id"]);


            return transfer;
        }
    }
}
