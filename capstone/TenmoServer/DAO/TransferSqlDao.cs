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

        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

      

        // Parameter = user_id since we need that value for our SQL query
        public Transfer GetBalance(int user_id)
        {

            // Instantiate transfer (all property values = initialized to zero) 
            Transfer transfer = new Transfer();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM account WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    SqlDataReader reader = cmd.ExecuteReader();


                    //We're getting just one row of data, so use an if statement (multiple rows = used for lists = uses while loop) 
                    if (reader.Read())
                    {
                        // translates sql data -> C# data
                        transfer = GetAccountFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return transfer;
        }


        public Transfer MakeTransaction(int userId, int receiverId, double amountToSend)
        {
            Transfer transfer = new Transfer();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM account WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transfer = GetAccountFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return transfer;

        }

        public Transfer UpdateSenderAccount(int userId, double amountToSend)
        {
            Transfer transfer = new Transfer();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();


                // Update Sender's balance (-amountToSend)
                SqlCommand cmd2 = new SqlCommand("update account set balance -= @amountToSend where user_id = @userId", conn);
                cmd2.Parameters.AddWithValue("@amountToSend", amountToSend);
                cmd2.Parameters.AddWithValue("@userId", userId);
                cmd2.ExecuteNonQuery();


                // Grab the sender's new data (and store in transfer object)
                SqlCommand cmd = new SqlCommand("select * from account where user_id = @user_id", conn);
                cmd.Parameters.AddWithValue("@user_id", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    transfer = GetAccountFromReader(reader);
                }

            }
            return transfer;
            
        }

        public Transfer UpdateReceiverAccount(int receiverId, double amountToSend)
        {
            Transfer transfer = new Transfer();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();


                // Update receiver's balance (-amountToSend)
                SqlCommand cmd3 = new SqlCommand("update account set balance += @amountToSend where user_id = @receiverId", conn);
                cmd3.Parameters.AddWithValue("@amountToSend", amountToSend);
                cmd3.Parameters.AddWithValue("@receiverId", receiverId);
                cmd3.ExecuteNonQuery();

                // Grab the receiver's new data (and store in transfer object)
                SqlCommand cmd = new SqlCommand("select * from account where user_id = @receiver_Id", conn);
                cmd.Parameters.AddWithValue("@receiver_id", receiverId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    transfer = GetAccountFromReader(reader);
                }

            }
            return transfer;

        }

    

        // Converts sql data -> C# data
        private Transfer GetAccountFromReader(SqlDataReader reader)
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
