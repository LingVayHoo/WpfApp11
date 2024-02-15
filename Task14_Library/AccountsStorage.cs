using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task14_Library
{
    public class AccountsStorage
    {
        private readonly string _fileName = "AccountDetails.txt";


        public List<IAccount<BankAccount, BankAccount>> GetDataBase()
        {
            List<IAccount<BankAccount, BankAccount>> bankAccounts = new List<IAccount<BankAccount, BankAccount>>();
            if (IsFileExists())
            {
                using (StreamReader sr = new StreamReader(_fileName, Encoding.Unicode))
                {
                    string line;
                    IAccount<BankAccount, BankAccount> account = null;

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] data = line.Split('#');

                        if (data[4] == "0")
                        {
                            account = new SimpleBankAccount(data);
                        }
                        else if (data[4] == "1")
                        {
                            account = new DepositBankAccount(data);
                        }

                        bankAccounts.Add(account);
                    }
                }
            }
            else
            {
                return null;
            }
            return bankAccounts;
        }

        public List<string> GetAccountNumbers()
        {
            List<IAccount<BankAccount, BankAccount>> bankAccounts = GetDataBase();
            List<string> data = new List<string>();

            if (bankAccounts != null)
            {
                foreach (IAccount<BankAccount, BankAccount> e in bankAccounts)
                {
                    data.Add(e.GetValue.AccountNumber);
                }
            }
            return data;
        }

        public IAccount<BankAccount, BankAccount> GetAccount(string accountNumber)
        {
            IAccount<BankAccount, BankAccount> bankAccount = null;
            List<IAccount<BankAccount, BankAccount>> bankAccounts = GetDataBase();
            foreach (IAccount<BankAccount, BankAccount> e in bankAccounts)
            {
                if (e.GetValue.AccountNumber == accountNumber)
                    bankAccount = e;
            }
            return bankAccount;
        }

        public IAccount<BankAccount, BankAccount> GetAccountByID(string OwnerID, int accountType)
        {
            IAccount<BankAccount, BankAccount> bankAccount = null;
            List<IAccount<BankAccount, BankAccount>> bankAccounts = GetDataBase();
            if (bankAccounts != null)
            {
                foreach (IAccount<BankAccount, BankAccount> e in bankAccounts)
                {
                    if (e.GetValue.OwnerID == OwnerID && e.GetValue.AccountTypeInt == accountType)
                        bankAccount = e;
                }
            }
            return bankAccount;
        }

        public void SaveAccount(string[] data)
        {
            List<IAccount<BankAccount, BankAccount>> bankAccounts = GetDataBase();
            IAccount<BankAccount, BankAccount> account = null;
            // Определяю, какой тип класса нужно создать
            if (data[4] == "0")
            {
                account = new SimpleBankAccount(data);
            }
            else if (data[4] == "1")
            {
                account = new DepositBankAccount(data);
            }

            //сохранение
            if (bankAccounts != null)
            {
                foreach (IAccount<BankAccount, BankAccount> e in bankAccounts)
                {
                    if (e.GetValue.AccountNumber == account.GetValue.AccountNumber)
                    {
                        bankAccounts.Remove(e);
                        break;
                    }
                }
                bankAccounts.Add(account);
            }
            else
            {
                bankAccounts = new List<IAccount<BankAccount, BankAccount>>
                {
                    account
                };
            }
            SaveDataBase(bankAccounts);
        }

        public void DeleteAccount(BankAccount account)
        {
            List<IAccount<BankAccount, BankAccount>> bankAccounts = GetDataBase();

            foreach (IAccount<BankAccount, BankAccount> e in bankAccounts)
            {
                if (e.GetValue.AccountNumber == account.AccountNumber)
                {
                    bankAccounts.Remove(e);
                    break;
                }
            }
            SaveDataBase(bankAccounts);
        }

        public void SaveDataBase(List<IAccount<BankAccount, BankAccount>> accounts)
        {
            using (StreamWriter sw = new StreamWriter(_fileName, false, Encoding.Unicode))
            {
                foreach (IAccount<BankAccount, BankAccount> e in accounts)
                {
                    sw.WriteLine(e.GetValue.ToString());
                }
            }
        }

        public bool IsAccountExists(string id, int typeValue)
        {
            bool result = false;
            List<IAccount<BankAccount, BankAccount>> bankAccounts = GetDataBase();
            if (bankAccounts != null)
            {
                foreach (IAccount<BankAccount, BankAccount> account in bankAccounts)
                {
                    if (account.GetValue.OwnerID == id &&
                        account.GetValue.AccountTypeInt == typeValue)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Метод изменения баланса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accountNumber"></param>
        /// <param name="creditsValue">Для уменьшения баланса значение должно быть отрицательным</param>
        public bool AddCredits(string accountNumber, float creditsValue)
        {
            bool result = false;
            List<IAccount<BankAccount, BankAccount>> bankAccounts = GetDataBase();
            foreach (IAccount<BankAccount, BankAccount> account in bankAccounts)
            {
                if (account.GetValue.AccountNumber == accountNumber)
                {
                    if (IsEnoughCredits(account.GetValue.AccountBalance, creditsValue))
                    {
                        account.GetValue.AccountBalance += creditsValue;
                        account.GetValue.Save();
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private bool IsEnoughCredits(float balance, float creditsValue)
        {
            if (balance + creditsValue >= 0) return true;
            else return false;
        }

        /// <summary>
        /// Метод для проверки существования файла
        /// </summary>
        /// <returns></returns>
        private bool IsFileExists()
        {
            FileInfo file = new FileInfo(_fileName);
            return file.Exists;
        }
    }
}
