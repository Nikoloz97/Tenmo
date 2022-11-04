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

      

        public Transfer GetBalance(int user_id)
        {
            Transfer transfer = new Transfer();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM account WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", user_id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transfer = GetBalanceFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return transfer;
        }


        public Transfer MakeTransaction(int userID, int receiverId, double amountToSend)
        {
            Transfer transfer = new Transfer();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM account WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userID);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        transfer = GetBalanceFromReader(reader);
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

                SqlCommand cmd2 = new SqlCommand("update account set balance -= @amountToSend where user_id = @userId", conn);
                cmd2.Parameters.AddWithValue("@amountToSend", amountToSend);
                cmd2.Parameters.AddWithValue("@userId", userId);
                cmd2.ExecuteNonQuery();
<<<<<<< HEAD
=======

                SqlCommand cmd = new SqlCommand("select * from account where user_id = @user_id", conn);
                cmd.Parameters.AddWithValue("@user_id", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    transfer = GetBalanceFromReader(reader);
                }
>>>>>>> 4930077c4ca8dd729f529ef0442ab657ace11808
            }
            return transfer;
            
        }

        public Transfer UpdateReceiverAccount(int receiverId, double amountToSend)
        {
            Transfer transfer = new Transfer();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
<<<<<<< HEAD
                SqlCommand cmd3 = new SqlCommand("update account set balance += @amountToSend where account_id = @receiverId");
=======

                

                SqlCommand cmd3 = new SqlCommand("update account set balance += @amountToSend where user_id = @receiverId");
>>>>>>> 4930077c4ca8dd729f529ef0442ab657ace11808
                cmd3.Parameters.AddWithValue("@amountToSend", amountToSend);
                cmd3.Parameters.AddWithValue("@receiverId", receiverId);
                cmd3.ExecuteNonQuery();
            }
            return transfer;

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
