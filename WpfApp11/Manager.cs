using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp11
{
    public class Manager
    {
        private readonly AccountsStorage _accountsStorage = new AccountsStorage();
        private readonly PersonDataStorage _personDataStorage = new PersonDataStorage();

        public List<Person> GetPersonDataBase()
        {
            return _personDataStorage.GetDataBase();
        }

        public void CreatePerson(string[] data)
        {
            Person person = new Person(data);
            person.Save();
        }

        public void OpenBankAccount<T>(string id)
            where T : IAccount<BankAccount, BankAccount>, new()
        {
            T account = new T();
            //bool IsAccountExists = false;
            //if (account is SimpleBankAccount) IsAccountExists = _accountsStorage.IsAccountExists(id, 0);
            //else if (account is DepositBankAccount) IsAccountExists = _accountsStorage.IsAccountExists(id, 1);
            if (!IsAccountExists<T>(id))
            {
                account.SetFields(id);
                Console.WriteLine(account.GetValue.ToString());
                account.GetValue.Save();
            }            
        }

        public bool IsAccountExists<T>(string id)
            where T : IAccount<BankAccount, BankAccount>, new()
        {
            T account = new T();
            bool isAccountExists = false;
            if (account is SimpleBankAccount) isAccountExists = _accountsStorage.IsAccountExists(id, 0);
            else if (account is DepositBankAccount) isAccountExists = _accountsStorage.IsAccountExists(id, 1);
            return isAccountExists;
        }

        public void CloseBankAccount(string accountNumber)
        {
            IAccount<BankAccount, BankAccount> account = _accountsStorage.GetAccount(accountNumber);
            account?.GetValue.Delete();
        }

        public bool CashTransfer(string outAccountNumber, string inAccountNumber, float creditsValue)
        {
            if(outAccountNumber != String.Empty && 
                _accountsStorage.AddCredits(outAccountNumber, -creditsValue))
            {
                _accountsStorage.AddCredits(inAccountNumber, creditsValue);
                return true;
            }
            else if(outAccountNumber == String.Empty)
            {
                _accountsStorage.AddCredits(inAccountNumber, creditsValue);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IAccount<BankAccount, BankAccount> GetAccountByIDAndType(string OwnerID, int accountType)
        {
            IAccount<BankAccount, BankAccount> account = _accountsStorage.GetAccountByID(OwnerID, accountType);
            if (account != null) return account;
            else return null;
        }
    }
}
