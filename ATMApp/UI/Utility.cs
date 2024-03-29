﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI
{
    public static class Utility
    {
        private static long _transId;
        private static CultureInfo _culture = new CultureInfo("en-US");

        public static long GetTransactionId()
        {
            return ++_transId;
        }

        public static string GetUserInput(string prompt)
        {
            Console.Write($"Enter {prompt}: ");
            return Console.ReadLine();
        }

        public static void PressEnterToContinue()
        {
            Console.WriteLine("\n\nPress Enter to continue...\n");
            Console.ReadLine();
        }

        public static void PrintMessage(string msg, bool success = true)
        {
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
            PressEnterToContinue();
        }

        public static string GetSecretInput(string prompt)
        {
            bool isPrompt = true;
            string asterics = "";

            StringBuilder input = new StringBuilder();

            while (true)
            {
                if (isPrompt)
                {
                    Console.Write(prompt);
                }
                isPrompt = false;

                ConsoleKeyInfo inputKey = Console.ReadKey(true);

                if (inputKey.Key == ConsoleKey.Enter)
                {
                    if (input.Length == 6)
                    {
                        break;
                    }
                    else
                    {
                        PrintMessage("\nPlease enter 6 digits", false);
                        input.Clear();
                        isPrompt = true;
                        continue;
                    }
                }
                if (inputKey.Key == ConsoleKey.Backspace && input.Length > 0 )
                {
                    input.Remove(input.Length - 1, 1);
                } 
                else if (inputKey.Key != ConsoleKey.Backspace)
                {
                    input.Append(inputKey.KeyChar);
                    Console.Write(asterics + "*");
                }
            }
            return input.ToString();
        }

        public static void PrintDotAnimation(int timer = 10)
        {
            for (int i = 0; i < timer; i++)
            {
                Console.Write(".");
                Thread.Sleep(200);
            }
            Console.Clear();
        }

        public static string FormatAmount(decimal amt)
        {
            return String.Format(_culture, "{0:C2}", amt);
        }
    }
}
