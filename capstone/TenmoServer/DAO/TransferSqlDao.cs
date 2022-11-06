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

        public Transfer LogTransfer (int user_id, int receiver_id, double amountToTransfer)
        {
            Transfer transfer = new Transfer();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();


                SqlCommand cmd = new SqlCommand("insert into transfer (transfer_type_id, transfer_status_id, account_from, account_to, amount)" +
                    "values(2, 2, (select account_id from account where user_id = @User_ID), (select account_id from account where user_id = @Receiver_ID), @AmountToTransfer)", conn);

                cmd.Parameters.AddWithValue("@User_ID", user_id);
                cmd.Parameters.AddWithValue("@Receiver_ID", receiver_id);
                cmd.Parameters.AddWithValue("@AmountToTransfer", amountToTransfer);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    transfer = GetTransferFromReader(reader);
                }

            }
            return transfer;

        }

        public List<Transfer> DisplaySendingLog (int user_id)
        {
            List<Transfer> transferList = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Sending out info...
                SqlCommand cmd = new SqlCommand("select transfer_id, username, amount from account join transfer on account.account_id = transfer.account_from join tenmo_user on account.user_id = tenmo_user.user_id where account.user_id = @userID", conn);

                cmd.Parameters.AddWithValue("@userID", user_id);
               
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Transfer transfer = GetTransferFromReader(reader);
                    transferList.Add(transfer);
                }

            }
            return transferList;

        }


        public List<Transfer> DisplayReceivingLog(int user_id)
        {
            List<Transfer> transferList = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select transfer_id, username, amount from account join transfer on account.account_id = transfer.account_to join tenmo_user on account.user_id = tenmo_user.user_id where account.user_id = @userID", conn);

                cmd.Parameters.AddWithValue("@userID", user_id);

                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Transfer transfer = GetTransferFromReader(reader);
                    transferList.Add(transfer);
                }

                return transferList;
            }
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

        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                Username = Convert.ToString(reader["username"]),
                TransferAmount = Convert.ToDouble(reader["amount"]),

                /*Transfer_type_id = Convert.ToInt32(reader["transfer_type_id"]),
                Transfer_status_id = Convert.ToInt32(reader["transfer_status_id"]),
                SenderAccountId = Convert.ToInt32(reader["account_from"]),
                RecieverAccountId = Convert.ToInt32(reader["account_to"]),*/
            };
            return transfer;
        }




        private Transfer GetAccountFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                UserId = Convert.ToInt32(reader["user_id"]),
                SenderAccountId = Convert.ToInt32(reader["account_id"]),
                Balance = Convert.ToDouble(reader["balance"]),

            };
            return transfer;
        }



    }
}
