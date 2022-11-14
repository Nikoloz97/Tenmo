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
        private Dictionary<string, int> MyDictionary = new Dictionary<string, int>();
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


                SqlCommand cmd = new SqlCommand("insert into transfer (transfer_type_id, transfer_status_id, account_from, account_to, amount) values(2, 2, (select account_id from account where user_id = @User_ID), (select account_id from account where user_id = @Receiver_ID), @AmountToTransfer)", conn);

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


        // The "to:"
        public List<Transfer> DisplaySendingLog (int user_id)
        {
            List<Transfer> transferList = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1. Get rows of account_id, transfer_id, and amount from which user sent money...
                SqlCommand cmd = new SqlCommand("select account_to, amount, transfer_id from transfer join account on transfer.account_from = account.account_id join tenmo_user on account.user_id = tenmo_user.user_id where account.user_id = @user_id; ", conn);

                cmd.Parameters.AddWithValue("@user_id", user_id);
               
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Transfer transfer = GetTransferTosFromReader(reader);
                    transferList.Add(transfer);
                }

                // 2. Adds username property for each item in list based on their account id's from step 1.... 
                // NOTE: Had to enable MARS (activated it in connection string in Startup.cs)
                foreach (Transfer Item in transferList)
            {

                SqlCommand cmd2 = new SqlCommand("select username from tenmo_user join account on tenmo_user.user_id = account.user_id where account_id = @account_to ", conn);


                cmd2.Parameters.AddWithValue("@account_to", Item.RecieverAccountId);

                SqlDataReader reader_2 = cmd2.ExecuteReader();


                while (reader_2.Read())
                {
                    Transfer transfer = GetTransferUsernameFromReader(reader_2);
                    Item.Username = transfer.Username;
                }
            }
            }

            return transferList;

        }


        public Transfer DisplayFullSendingLog(int user_id, int transfer_id)
        {
            Transfer transfer = new Transfer();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1. Get a row of account_id, transfer_id, amount, AND transfer type description, transfer status description, from which user sent money based on their transfer_id...
                SqlCommand cmd = new SqlCommand("select  account_to, amount, transfer_id, transfer_status_desc, transfer_type_desc from transfer join account on transfer.account_from = account.account_id join tenmo_user on account.user_id = tenmo_user.user_id join transfer_status on transfer.transfer_status_id = transfer_status.transfer_status_id join transfer_type on transfer.transfer_type_id = transfer_type.transfer_type_id where account.user_id = @user_id and transfer_id = @transfer_id ; ", conn);

                cmd.Parameters.AddWithValue("@user_id", user_id);
                cmd.Parameters.AddWithValue("@transfer_id", transfer_id);



                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.Read())
                {
                    transfer = GetFullTransferTosFromReader(reader);
                }

                // 2. Adds username property based on their account id's from step 1.... 

                    SqlCommand cmd2 = new SqlCommand("select username from tenmo_user join account on tenmo_user.user_id = account.user_id where account_id = @account_to ", conn);


                    cmd2.Parameters.AddWithValue("@account_to", transfer.RecieverAccountId);

                    SqlDataReader reader_2 = cmd2.ExecuteReader();


                    if (reader_2.Read())
                    {
                        transfer.Username = GetTransferUsernameFromReader(reader_2).Username;
                    }
 
            }

            return transfer;

        }

        // The "From:"
        public List<Transfer> DisplayReceivingLog(int user_id)
        {
            List<Transfer> transferList = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1. Get rows of account_id, transfer_id, and amount from which user sent money...
                SqlCommand cmd = new SqlCommand("select account_from,amount, transfer_id from transfer join account on transfer.account_to = account.account_id join tenmo_user on account.user_id = tenmo_user.user_id where account.user_id = @user_id; ", conn);

                cmd.Parameters.AddWithValue("@user_id", user_id);

                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Transfer transfer = GetTransferFromsFromReader(reader);
                    transferList.Add(transfer);
                }


                // 2. Adds username property for each item in list based on their account id's from step 1.... 
                foreach (Transfer Item in transferList)
                {

                    SqlCommand cmd2 = new SqlCommand("select username from tenmo_user join account on tenmo_user.user_id = account.user_id where account_id = @account_from", conn);


                    cmd2.Parameters.AddWithValue("@account_from", Item.SenderAccountId);

                    SqlDataReader reader_2 = cmd2.ExecuteReader();


                    while (reader_2.Read())
                    {
                        Transfer transfer = GetTransferUsernameFromReader(reader_2);
                        Item.Username = transfer.Username;
                    }

                }
            }
            return transferList;
        }

        public Transfer DisplayFullReceivingLog(int user_id, int transfer_id)
        {
            Transfer transfer = new Transfer();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1. Get row of account_id, transfer_id, and amount from which user sent money based on their transfer_id...
                SqlCommand cmd = new SqlCommand("select account_from, amount, transfer_id, transfer_status_desc, transfer_type_desc from transfer join account on transfer.account_to = account.account_id join tenmo_user on account.user_id = tenmo_user.user_id join transfer_status on transfer.transfer_status_id = transfer_status.transfer_status_id join transfer_type on transfer.transfer_type_id = transfer_type.transfer_type_id where account.user_id = @user_id and transfer_id = @transfer_id; ", conn);

                cmd.Parameters.AddWithValue("@user_id", user_id);
                cmd.Parameters.AddWithValue("@transfer_id", transfer_id);


                SqlDataReader reader = cmd.ExecuteReader();


            if (reader.Read())
            {
                transfer = GetFullTransferFromsFromReader(reader);
            }

            // 2. Adds username property based on their account id's from step 1.... 

            SqlCommand cmd2 = new SqlCommand("select username from tenmo_user join account on tenmo_user.user_id = account.user_id where account_id = @account_from ", conn);


            cmd2.Parameters.AddWithValue("@account_from", transfer.SenderAccountId);

            SqlDataReader reader_2 = cmd2.ExecuteReader();


            if (reader_2.Read())
            {
                transfer.Username = GetTransferUsernameFromReader(reader_2).Username;
            }

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


        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                Transfer_type_id = Convert.ToInt32(reader["transfer_type_id"]),
                Transfer_status_id = Convert.ToInt32(reader["transfer_status_id"]),
                SenderAccountId = Convert.ToInt32(reader["account_from"]),
                RecieverAccountId = Convert.ToInt32(reader["account_to"]),
                TransferAmount = Convert.ToDouble(reader["amount"]),
               
            };
            return transfer;
        }


        private Transfer GetTransferTosFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                TransferAmount = Convert.ToDouble(reader["amount"]),
                RecieverAccountId = Convert.ToInt32(reader["account_to"]),
               
            };
            return transfer;
        }

        private Transfer GetFullTransferTosFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                TransferAmount = Convert.ToDouble(reader["amount"]),
                RecieverAccountId = Convert.ToInt32(reader["account_to"]),
                status_desc = Convert.ToString(reader["transfer_status_desc"]),
                type_desc = Convert.ToString(reader["transfer_type_desc"]),
            };
            return transfer;
        }

        private Transfer GetTransferFromsFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                TransferAmount = Convert.ToDouble(reader["amount"]),
                SenderAccountId = Convert.ToInt32(reader["account_from"]),
            };
            return transfer;
        }

        private Transfer GetFullTransferFromsFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                TransferAmount = Convert.ToDouble(reader["amount"]),
                SenderAccountId = Convert.ToInt32(reader["account_from"]),
                status_desc = Convert.ToString(reader["transfer_status_desc"]),
                type_desc = Convert.ToString(reader["transfer_type_desc"]),

            };
            return transfer;
        }

        private Transfer GetTransferUsernameFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                Username = Convert.ToString(reader["username"]),
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
