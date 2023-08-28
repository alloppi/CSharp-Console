using ATMApp.Domain.Entities;
using ATMApp.Domain.Enums;
using ATMApp.Domain.Interfaces;
using ATMApp.UI;
using ConsoleTables;

namespace ATMApp
{
    public class ATMApp : IUserLogin, IUserAccountActions, ITransaction
    { 
        private List<UserAccount> userAcccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _ListOfTransactions;
        private const decimal minimumKeptAmount = 100;
        private readonly AppScreen screen;
        private bool mainMenu = true;

        public ATMApp()
        {
            screen = new AppScreen();
        }

        public void Run()
        {
            AppScreen.Welcome();
            CheckUserCardNumAndPassword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            while (mainMenu)
            {
                AppScreen.DisplayAppMenu();
                ProcessMenuoption();
            }
        }

        public void InitializeData()
        {
            userAcccountList = new List<UserAccount>
            {
                new UserAccount { Id = 1, FullName = "Alan Chan", AccountNumber = 123456, CardNumber = 321321, CardPin = 123123, AccountBalance = 10000.00m, IsLocked = false },
                new UserAccount { Id = 2, FullName = "Bob Lee", AccountNumber = 456789, CardNumber = 111555, CardPin = 123789, AccountBalance = 30000.00m, IsLocked = false },
                new UserAccount { Id = 3, FullName = "Jackie Lo", AccountNumber = 654321, CardNumber = 222444, CardPin = 123456, AccountBalance = 40000.00m, IsLocked = true },
                new UserAccount { Id = 4, FullName = "Fiona Chung", AccountNumber = 123457, CardNumber = 321322, CardPin = 123457, AccountBalance = 1000.00m, IsLocked = false },
            };
            _ListOfTransactions = new List<Transaction>();
        }

        public void CheckUserCardNumAndPassword()
        {
            bool isCorrrectLogin = false;

            while (isCorrrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach (UserAccount account in userAcccountList)
                {
                    selectedAccount = account;
                    if (inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.TotalLogin++;

                        if (inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;

                            if (selectedAccount.IsLocked || selectedAccount.TotalLogin > 3) 
                            {
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrrectLogin = true;
                                break;
                            }
                        }
                    }
                }
                if (isCorrrectLogin == false)
                {
                    Utility.PrintMessage("\nInvalid card number or PIN.", false);
                    selectedAccount.TotalLogin++;
                    selectedAccount.IsLocked = (selectedAccount.TotalLogin == 3);
                    if (selectedAccount.IsLocked)
                    {
                        AppScreen.PrintLockScreen();
                    }
                }
                Console.Clear();
            }
        }

        private void ProcessMenuoption()
        {
            switch(Validator.Convert<int>("an option"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    MakeWithdrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    var interTransfer = screen.InternalTransferForm();
                    ProcessInternalTransfer(interTransfer);
                    break;
                case (int)AppMenu.ViewTransaction:
                    ViewTransaction();
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utility.PrintMessage("You have succesfully logged out. Please collect " +
                        "your ATM card.");
                    mainMenu = false;
                    break;
                default:
                    Utility.PrintMessage("Invalid option", false);
                    break;
            }
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your account balance is: {Utility.FormatAmount(selectedAccount.AccountBalance)}");
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\nOnly multiple of 10 and 50 dollars allowws.\n");
            var transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");

            // Simulate counting
            Console.WriteLine("\nCheccking and Counting bank notes.");
            Utility.PrintDotAnimation();
            Console.WriteLine("");

            // Some guard clause
            if ( transaction_amt < 0 )
            {
                Utility.PrintMessage("Amount needs to be greater than zero. try again.", false);
                return;
            }
            if ( transaction_amt % 10 != 0 ) 
            {
                Utility.PrintMessage("Enter deposit amount in multiple of 10 or 50. Try again", false);
                return;
            }
            if (PreviewBankNotesCount(transaction_amt) == false)
            {
                Utility.PrintMessage($"You have cancelled your action.", false);
                return;            
            }

            // Bind transaction details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transaction_amt, "");

            // Update account balance
            selectedAccount.AccountBalance += transaction_amt;

            // Print success message
            Utility.PrintMessage($"Your desposit of {Utility.FormatAmount(transaction_amt)} was successful", true);
        }

        public void MakeWithdrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = AppScreen.SelectAmount();
            if ( selectedAmount == -1) 
            {
                MakeWithdrawal();
                return;
            }
            else if ( selectedAmount != 0 )
            {
                transaction_amt = selectedAmount;
            }
            else
            { 
                transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}"); 
            }

            // input validation
            if ( transaction_amt <= 0 )
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again.", false);
                return;
            }
            if ( transaction_amt % 10 != 0 )
            {
                Utility.PrintMessage("You can withfraw amount in multiples of 50 or 10 dollars. Try again", false);
                return;
            }

            // Business logic validations
            if ( transaction_amt > selectedAccount.AccountBalance ) 
            {
                Utility.PrintMessage($"Withdrawal failed. Your balance is too low to withdraw " +
                    $"{Utility.FormatAmount(transaction_amt)}", false);
                return;
            }
            if ( selectedAccount.AccountBalance - transaction_amt < minimumKeptAmount ) 
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have " +
                    $"minimum {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }

            // Bind withdrawal details to treansaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, -transaction_amt, "");

            // Update account balance
            selectedAccount.AccountBalance -= transaction_amt;

            // success message
            Utility.PrintMessage($"You have successfully withdrawn " +
                $"{Utility.FormatAmount(transaction_amt)}.", true);
        }

        private bool PreviewBankNotesCount(int amount)
        {
            int fiftyNotesCount = amount / 50;
            int tenNotesCount = (amount % 50) / 10;

            Console.WriteLine("\nSummary");
            Console.WriteLine("-------");
            Console.WriteLine($"{AppScreen.cur}50 X {fiftyNotesCount} = {50 * fiftyNotesCount}");
            Console.WriteLine($"{AppScreen.cur}10 X {tenNotesCount} = {10 * tenNotesCount}");
            Console.WriteLine($"Total amount: {Utility.FormatAmount(amount)}\n\n");

            int opt = Validator.Convert<int>("1 to confirm");
            return opt.Equals(1);
        }

        public void InsertTransaction(long _UserBankAccountId, TransactionType _transType, decimal _transAmount, string _desc)
        {
            // Create a new transaction object
            var transaction = new Transaction()
            {
                TransactionId = Utility.GetTransactionId(),
                UserBankAccountId = _UserBankAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _transType,
                TransactionAmount = _transAmount,
                Description = _desc
            };

            // Add transaction object to the list
            _ListOfTransactions.Add(transaction);
        }

        public void ViewTransaction()
        {
           var filteredTransactionList = _ListOfTransactions.Where(t => t.UserBankAccountId == selectedAccount.Id).ToList();
            // Check if there's a transaction
            if (filteredTransactionList.Count <= 0)
            {
                Utility.PrintMessage("You have no transaction yet.", true);
            }
            else
            {
                var table = new ConsoleTable("Id", "Transaction Date", "Type", "Descriptions", "Amount " + AppScreen.cur );
                foreach ( var tran in filteredTransactionList ) 
                {
                    table.AddRow(tran.TransactionId, tran.TransactionDate, tran.TransactionType, tran.Description, tran.TransactionAmount);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.PrintMessage($"You have {filteredTransactionList.Count} transaction(s)", true);
            }
        }

        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if ( internalTransfer.TransferAmount <= 0 ) 
            {
                Utility.PrintMessage("Amount needs to be more than zero. Try again.");
                return;
            }

            // Check sender's account balance
            if ( internalTransfer.TransferAmount > selectedAccount.AccountBalance )
            {
                Utility.PrintMessage($"Tranfer failed. You do not have enough balance" +
                    $" to tranfer {Utility.FormatAmount(internalTransfer.TransferAmount)}", false);
                return;
            }

            // Check the minimum kelp amount
            if (selectedAccount.AccountBalance - internalTransfer.TransferAmount < minimumKeptAmount ) 
            {
                Utility.PrintMessage($"Transfer failed. Your account need to have minimum " +
                    $" {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }

            // Check receiver's account number is valid
            var selectedBankAccountReciever = (from userAcc in userAcccountList
                                               where userAcc.AccountNumber == internalTransfer.ReceiptBankAccountNumber
                                               select userAcc).FirstOrDefault();
            if ( selectedBankAccountReciever == null )
            {
                Utility.PrintMessage("Transfer failed. Receiver bank account number is invalid.", false);
                return;
            }

            // Check Receiver's name
            if ( selectedBankAccountReciever.FullName != internalTransfer.ReceiptBankAccountName )
            {
                Utility.PrintMessage("Transfer Failed. Recipient's bank account name does not match", false);
                return;
            }

            // Add transaction to transactions record to sender
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, "Transfered" +
                $"to {selectedBankAccountReciever.AccountNumber} ({selectedBankAccountReciever.FullName})");

            // Update sender's account balance
            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;

            // Add transaction record to receiver
            InsertTransaction(selectedBankAccountReciever.Id, TransactionType.Transfer, internalTransfer.TransferAmount,
                "Transfered from " + $"{ selectedAccount.AccountNumber} ({selectedAccount.FullName})");

            // Update receiver's account balance
            selectedBankAccountReciever.AccountBalance += internalTransfer.TransferAmount;

            // Print success message
            Utility.PrintMessage($"You have successfully transfered" +
                $" {Utility.FormatAmount(internalTransfer.TransferAmount)} to " +
                $"{internalTransfer.ReceiptBankAccountName}", true);
        }
    }
}