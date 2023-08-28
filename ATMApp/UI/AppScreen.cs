using ATMApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI
{
    public class AppScreen
    {
        internal const string cur = "US $";

        internal static void Welcome()
        {
            // Clear the console screen
            Console.Clear();

            // Reset the console color
            Console.ResetColor();

            // Set the title of the console window
            Console.Title = "ATM Console App";

            // Set the text color of foreground color to white
            Console.ForegroundColor = ConsoleColor.White;

            // Set the welcome message
            Console.WriteLine("\n\n---------------Welcome to ATM Console App---------------\n\n");

            // Prompt the user to insert ATM card
            Console.WriteLine("Please insert your ATM card");
            Console.WriteLine("Note: Actual ATM machine will accept and validate" +
                " a physical ATM card, read the card number and validate it.");
            Utility.PressEnterToContinue();
        }

        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();
            tempUserAccount.CardNumber = Validator.Convert<long>("your ATM card number");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter your card PIN: "));
            return tempUserAccount;
        }

        internal static void LoginProgress() 
        {
            Console.WriteLine("\nChecking card number and PIN...");
            Utility.PrintDotAnimation();
        }

        internal static void PrintLockScreen() 
        {
            Console.Clear();
            Utility.PrintMessage("Your account is locked. Please go to the nearest branch" +
                " to unlock your account. Thank you.", true);
            Utility.PressEnterToContinue();
            Environment.Exit(1);
        }

        internal static void WelcomeCustomer(string fullName)
        {
            Console.WriteLine($"Welecome back, {fullName}");
            Utility.PressEnterToContinue();
        }

        internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("--------My ATM App Menu--------");
            Console.WriteLine(":                             :");
            Console.WriteLine("1. Acccount Balance           :");
            Console.WriteLine("2. Cash Deposit               :");
            Console.WriteLine("3. Withdrawal                 :");
            Console.WriteLine("4. Transfer                   :");
            Console.WriteLine("5. Transaction                :");
            Console.WriteLine("6. logout                     :");
            Console.WriteLine(":                             :");
        }

        internal static void LogoutProgress() 
        {
            Console.WriteLine("Thank you for using My ATM App.");
            Utility.PrintDotAnimation();
            Console.Clear();
        }

        internal static int SelectAmount()
        {
            Console.WriteLine("");
            Console.WriteLine(":1.{0}10     5.{0}100", cur);
            Console.WriteLine(":2.{0}20     6.{0}150", cur);
            Console.WriteLine(":3.{0}30     7.{0}200", cur);
            Console.WriteLine(":4.{0}50     8.{0}500", cur);
            Console.WriteLine(":0.Other", cur);
            Console.WriteLine("");

            int selectedAmount = Validator.Convert<int>("option:");
            switch(selectedAmount)
            {
                case 1: 
                    return 10;
                    break;
                case 2:
                    return 20;
                    break;
                case 3:
                    return 30;
                    break;
                case 4:
                    return 50;
                    break;
                case 5:
                    return 100;
                    break;
                case 6:
                    return 150;
                    break;
                case 7:
                    return 200;
                    break;
                case 8:
                    return 500;
                    break;
                case 0:
                    return 0;
                    break;
                default:
                    Utility.PrintMessage("Invalid input. Try again.", false);
                    return -1;
                    break;
            }
        }
        internal InternalTransfer InternalTransferForm()
        {
            var internalTransfer = new InternalTransfer();
            internalTransfer.ReceiptBankAccountNumber = Validator.Convert<long>("recipient's account number");
            internalTransfer.TransferAmount = Validator.Convert<decimal>($"amount {cur}");
            internalTransfer.ReceiptBankAccountName = Utility.GetUserInput("recipient's name");
            return internalTransfer;
        }

    }
}
