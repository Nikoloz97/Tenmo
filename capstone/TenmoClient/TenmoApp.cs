﻿using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;


namespace TenmoClient
{
    public class TenmoApp
    {
        private ApiUser currentUser = null;
       
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;

        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {
                //view account balance 
                GetBalance();
            }

            if (menuSelection == 2)
            {
                // View your past transfers

                DisplaySenderLog();
                DisplayReceivingLog();
                console.Pause();

            }

            if (menuSelection == 3)
            {
                // View your pending requests
            }

            if (menuSelection == 4)
            {
               GetUsers();

                // TO DO: Merge these two functions into one 
               UpdateSenderAccount();
               UpdateReceiverAccount();


            }

            if (menuSelection == 5)
            {
                // Request TE bucks
            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }

        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    console.PrintSuccess("You are now logged in");
                    currentUser = user;
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }

        private void GetBalance()
        {
            try
            {
                Transfer transfers = tenmoApiService.GetAccountBalance(currentUser);
                if (transfers != null)
                {
                    console.PrintBalance(transfers);
                }
            }
            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }

        private void GetUsers()
        {
            try
            {
                List<ApiUser> users = tenmoApiService.GetUser();
                if (users != null)
                {
                    console.PrintUsers(users);
                }
            }
            catch (Exception ex)
            {
                console.PrintError(ex.Message);
            }
            console.Pause();
        }

        //public void MakeTransaction()
        //{
        //    Transfer transfer = tenmoApiService.MakeTransaction(currentUser, )
        //}

        public void UpdateSenderAccount()
        {
          Transfer transfer = new Transfer();

            // GetAccountBalance = returns transfer object with user's balance
          transfer.Balance = tenmoApiService.GetAccountBalance(currentUser).Balance;

            // currentUser = created in "Login" function above in this file
          transfer.UserId = currentUser.UserId;

            // console = refers to tenmoconsoleservice
            // prompts for user to set transfer's receiverID and transferAmount
          transfer = console.PromptAmountandReceiver(transfer);

          tenmoApiService.LogTransfer(transfer.UserId, transfer.ReceiverId, transfer.TransferAmount);

          transfer = tenmoApiService.UpdateSenderAccount(currentUser, transfer.TransferAmount);

          console.PrintUserBalance(transfer);
        }




        public void UpdateReceiverAccount()
        {
            Transfer transfer = new Transfer();

            transfer.UserId = currentUser.UserId;

            transfer.Balance = tenmoApiService.GetAccountBalance(currentUser).Balance;

            // console = tenmoconsoleservice
            // prompts for user to set transfer's receiverID and transferAmount
            transfer = console.PromptAmountandReceiver(transfer);


            // TenmoApiService is going to "call" the controller
            transfer.Balance = tenmoApiService.UpdateReceiverAccount(transfer.ReceiverId, transfer.TransferAmount).Balance;

            console.PrintReceiverBalance(transfer);
        }


        public void DisplaySenderLog()
        {
            List<Transfer> transferList = new List<Transfer>();

            transferList = tenmoApiService.DisplaySendingLog(currentUser.UserId);


            foreach (Transfer transfer in transferList)
            {
                Console.WriteLine($"TransferID: {transfer.TransferId}, From: {transfer.Username}, Amount: {transfer.TransferAmount:C2}");
            }

        }

        public void DisplayReceivingLog()
        {
            List<Transfer> transferList = new List<Transfer>();

            transferList = tenmoApiService.DisplayReceveingLog(currentUser.UserId);

            foreach (Transfer transfer in transferList)
            {
                Console.WriteLine($"TransferID: {transfer.TransferId}, To: {transfer.Username}, Amount: {transfer.TransferAmount:C2}");

            }
        }

    }
}

