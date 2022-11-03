﻿using System;
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


        public void MakeTransaction(int userID, int receiverId, double amountToSend)
        {
            Transfer transfer = new Transfer();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("insert into transfer (transfer_type_id, transfer_status_id, account_from, account_to, amount)" +
                                                                            "(values 2, 2, (select account_id from account where user_id = @userId), " +
                                                                            "(select account_id from account where user_id = @receiverId), @amountToSend", conn);
                    cmd.Parameters.AddWithValue("@userId", userID);
                    cmd.Parameters.AddWithValue("@recieverId", receiverId);
                    cmd.Parameters.AddWithValue("amountToSend", amountToSend);
                    SqlDataReader reader = cmd.ExecuteReader();
                                     
                }
            }
            catch (SqlException)
            {
                throw;
            }

            

        }

        public void UpdateSenderAccount(int userId, double amountToSend)
        {
             

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd2 = new SqlCommand("update account set balance -= @amountToSend where account_id = @userId");
                cmd2.Parameters.AddWithValue("@amountToSend", amountToSend);
                cmd2.Parameters.AddWithValue("@userId", userId);
                cmd2.ExecuteNonQuery();

                
            }

            
        }

        public void UpdateReceiverAccount(int receiverId, double amountToSend)
        {


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                

                SqlCommand cmd3 = new SqlCommand("update account set balance += @amountToSend where account_id = @receiverId");
                cmd3.Parameters.AddWithValue("@amountToSend", amountToSend);
                cmd3.Parameters.AddWithValue("@receiverId", receiverId);
                cmd3.ExecuteNonQuery();
            }


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
