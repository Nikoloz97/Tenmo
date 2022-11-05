using System;
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

      

        public Transfer PromptAmountandReceiver(Transfer transfer)
        {
            transfer.ReceiverId = PromptForInteger("Enter userid for who you want to send funds");

            transfer.TransferAmount = PromptForDouble("Enter amount to send");

            return transfer;
        }


        public void PrintBalance(Transfer transfer)
        {
            Console.WriteLine($"Your current balance: {transfer.Balance}");
            Pause();
        }

        public void PrintUserBalance(Transfer transfer)
        {
            transfer.Balance -= transfer.TransferAmount;
            Console.WriteLine($"Your current balance: {transfer.Balance}");
            Pause();
        }


        public void PrintReceiverBalance(Transfer transfer)
        {
            transfer.Balance += transfer.TransferAmount;
            Console.WriteLine($"Receiver's balance is now: {transfer.Balance}");
            Pause();
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
