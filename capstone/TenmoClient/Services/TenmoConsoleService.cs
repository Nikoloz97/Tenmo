﻿using System;
using System.Collections.Generic;
using System.Threading;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoConsoleService : ConsoleService
    {
        /************************************************************
            Print methods
        ************************************************************/
        public void PrintLoginMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }

        public void PrintMainMenu(string username)
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine($"Hello, {username}!");
            Console.WriteLine("1: View your current balance");
            Console.WriteLine("2: View your past transfers");
            Console.WriteLine("3: View your pending requests");
            Console.WriteLine("4: Send TE bucks");
            Console.WriteLine("5: Request TE bucks");
            Console.WriteLine("6: Log out");
            Console.WriteLine("0: Exit");
            Console.WriteLine("---------");
        }
        public LoginUser PromptForLogin()
        {
            string username = PromptForString("User name");
            if (String.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            string password = PromptForHiddenString("Password");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        public void PrintBalance(Transfer transfer)
        {
            Console.WriteLine($"Your current balance is: {transfer.Balance}");
            
        }

        public Transfer PrintSelectUser()
        {
            int idInput = PromptForInteger("Id for the user you're sending to:");

            double receiverBalance = 0;

            Transfer receivingUser = new Transfer
            {
                UserId = idInput,
                Balance = receiverBalance
           
            };

            return receivingUser;
        }

        public Transfer PrintAmountToTransfer(Transfer transfer)
        {
            transfer.ReceiverId = PromptForInteger("Enter userid for who you want to send money to: ");

            transfer.UserInput = PromptForDouble("Enter amount to send: ");

            transfer.Balance -= transfer.UserInput;
           // Console.WriteLine($"current balance {transfer.Balance}");
            Thread.Sleep(10000);
            return transfer;
        }



        public void PrintUsers(List<ApiUser> users)
        {
            foreach (ApiUser user in users)
            {
                Console.WriteLine($"ID: {user.UserId}");
                Console.WriteLine($"Name: {user.Username}");
            }
           
        }
    }
}
