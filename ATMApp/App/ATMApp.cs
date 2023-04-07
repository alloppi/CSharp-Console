using ATMApp.Domain.Entities;
using ATMApp.Domain.Interfaces;
using ATMApp.UI;

namespace ATMApp
{
    public class ATMApp : IUserLogin
    { 
        private List<UserAccount> userAcccountList;
        private UserAccount selectedAccount;

        public void InitializeData()
        {
            userAcccountList = new List<UserAccount>
            {
                new UserAccount { Id = 1, FullName = "Alan Chan", AccountNumber = 123456, CardNumber = 321321, CardPin = 123123, AccountBalance = 10000.00m, IsLocked = false },
                new UserAccount { Id = 2, FullName = "Bob Lee", AccountNumber = 456789, CardNumber = 111555, CardPin = 123789, AccountBalance = 30000.00m, IsLocked = false },
                new UserAccount { Id = 3, FullName = "Jackie Lo", AccountNumber = 654321, CardNumber = 222444, CardPin = 123456, AccountBalance = 40000.00m, IsLocked = true },
                new UserAccount { Id = 4, FullName = "Fiona Chung", AccountNumber = 123457, CardNumber = 321322, CardPin = 123457, AccountBalance = 1000.00m, IsLocked = false },
            };
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

        public void Welcome()
        {
            Console.WriteLine($"Welecome back, {selectedAccount.FullName}");
        }
    }
}