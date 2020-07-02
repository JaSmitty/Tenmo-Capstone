using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using TenmoClient.Data;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static readonly RestClient client = new RestClient();
        private readonly static string API_BASE_URL = "https://localhost:44315/";

        static void Main(string[] args)
        {
            Run();
        }
        private static void Run()
        {
            while (true)
            {
                int loginRegister = -1;
                while (loginRegister != 1 && loginRegister != 2)
                {
                    Console.WriteLine("Welcome to TEnmo!");
                    Console.WriteLine("1: Login");
                    Console.WriteLine("2: Register");
                    Console.WriteLine("0: Exit");
                    Console.Write("Please choose an option: ");

                    if (!int.TryParse(Console.ReadLine(), out loginRegister))
                    {
                        Console.WriteLine("Invalid input. Please enter only a number.");
                    }
                    else if (loginRegister == 0)
                    {
                        Environment.Exit(0);
                    }
                    else if (loginRegister == 1)
                    {
                        while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                        {
                            LoginUser loginUser = consoleService.PromptForLogin();
                            API_User user = authService.Login(loginUser);
                            if (user != null)
                            {
                                UserService.SetLogin(user);
                            }
                        }
                    }
                    else if (loginRegister == 2)
                    {
                        bool isRegistered = false;
                        while (!isRegistered) //will keep looping until user is registered
                        {
                            LoginUser registerUser = consoleService.PromptForLogin();
                            isRegistered = authService.Register(registerUser);
                            if (isRegistered)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Registration successful. You can now log in.");
                                loginRegister = -1; //reset outer loop to allow choice for login
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }

                MenuSelection();
            }
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                    menuSelection = -1;
                }
                else if (menuSelection == 1)
                {
                    // View your current balance
                    RestRequest request = new RestRequest(API_BASE_URL + "balance");
                    IRestResponse<decimal> response = client.Get<decimal>(request);
                    CheckResponse(response);
                    Console.WriteLine(response.Data);
                }
                else if (menuSelection == 2)
                {
                    // View your past transfers

                }
                else if (menuSelection == 3)
                {
                    // View your pending requests

                }
                else if (menuSelection == 4)
                {
                    // Send TE bucks

                }
                else if (menuSelection == 5)
                {
                    // Request TE bucks

                }
                else if (menuSelection == 6)
                {
                    // Log in as different user
                    Console.WriteLine("");
                    UserService.SetLogin(new API_User()); //wipe out previous login info
                    return; //return to entry point
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }



            }


        }

        private static void CheckResponse(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception("Error occurred - unable to reach server.");
            }

            if (!response.IsSuccessful)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Authorization is required for this option. Please log in.");
                }
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new Exception("You do not have permission to perform the requested action");
                }

                throw new Exception($"Error occurred - received non-success response: {response.StatusCode} ({(int)response.StatusCode})");
            }
        }
    }
}
