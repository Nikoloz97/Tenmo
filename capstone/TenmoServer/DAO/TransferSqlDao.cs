using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao

    {
        private readonly string connectionString;
        public double GetBalance(int user_id)
        {
            double balance = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance FROM account WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        balance = GetBalanceFromReader(reader).Balance;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return balance;
        }

        private Transfer GetBalanceFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                UserId = Convert.ToInt32(reader["user_id"]),
                AccountId = Convert.ToInt32(reader["account_id"]),
                Balance = Convert.ToDouble(reader["balance"]),

            };
            return transfer;
        }

    }
}
