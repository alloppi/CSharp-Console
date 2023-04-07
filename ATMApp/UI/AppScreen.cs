using ATMApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI
{
    public static class AppScreen
    {
        internal static void Welcome()
        {
            // Clear the console screen
            Console.Clear();

            // Set the title of the console window
            Console.Title = "My ATM App";

            // Set the text color or foreground color to white
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("\n\n---------------Welcome to My ATM App---------------\n\n");

            // prompt the user to insert ATM card
            Console.WriteLine("Please insert your ATM card");
            Console.WriteLine("Note: Actual ATM machine will accept and validate" +
                " a physical ATM card, read the card number and validate it.");
            Utility.PressEnterToContinue();
        }

        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();
            tempUserAccount.CardNumber = Validator.Convert<long>("Your ATM card number");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter your card PIN"));
            return tempUserAccount;
        }

        internal static void LoginProgress() 
        {
            Console.WriteLine("\nChecking card number and PIN");
            Utility.PrintDotAnimation();
        }

        internal static void PrintLockScreen() 
        {
            Console.Clear();
            Utility.PrintMessage("Your account is locked. Please go to the nearest branch" +
                " to unlock your account.", true);
            Utility.PressEnterToContinue();
            Environment.Exit(1);
        }
    }
}
