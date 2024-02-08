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
        private readonly ActionLog _actionLog = new ActionLog();
        public event Action<string> OnActionHappened;
        private string _name;

        public string Name
        { 
            get { return _name; } 
            set {  _name = value; } 
        }

        public Manager(string name)
        {
            Name = name;
        }

        public List<Person> GetPersonDataBase()
        {
            return _personDataStorage.GetDataBase();
        }

        public void CreatePerson(string[] data)
        {
            Person person = new Person(data);
            OnActionHappened?.Invoke("Клиент создан!");
            _actionLog.SaveStringAction(SaveActionLog("Клиент создан", "-", "-"));
            person.Save();
        }

        public void OpenBankAccount<T>(string id)
            where T : IAccount<BankAccount, BankAccount>, new()
        {
            T account = new T();
            if (!IsAccountExists<T>(id))
            {
                account.SetFields(id);
                account.GetValue.Save();
            }
            OnActionHappened?.Invoke("Счет открыт!");
            _actionLog.SaveStringAction(SaveActionLog("Счет открыт", "-", "-"));
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
            OnActionHappened?.Invoke("Счет удален!");
            _actionLog.SaveStringAction(SaveActionLog("Счет удален", "-", "-"));
            IAccount<BankAccount, BankAccount> account = _accountsStorage.GetAccount(accountNumber);
            account?.GetValue.Delete();
        }

        public bool CashTransfer(string outAccountNumber, string inAccountNumber, float creditsValue)
        {
            if(outAccountNumber != String.Empty && 
                _accountsStorage.AddCredits(outAccountNumber, -creditsValue))
            {
                _accountsStorage.AddCredits(inAccountNumber, creditsValue);
                OnActionHappened("Перевод выполнен!");
                _actionLog.SaveStringAction(
                    SaveActionLog("Перевод со счета", outAccountNumber, creditsValue.ToString()));
                _actionLog.SaveStringAction(
                    SaveActionLog("Перевод на счет", inAccountNumber, creditsValue.ToString()));
                return true;
            }
            else if(outAccountNumber == String.Empty)
            {
                _accountsStorage.AddCredits(inAccountNumber, creditsValue);
                OnActionHappened("Счет успешно пополнен!");
                _actionLog.SaveStringAction(
                    SaveActionLog("Пополнение", inAccountNumber, creditsValue.ToString()));
                return true;
            }
            else
            {
                OnActionHappened("Недостаточно средств!");
                _actionLog.SaveStringAction(
                    SaveActionLog
                    ("Отменено, недостаточно средств", inAccountNumber, creditsValue.ToString()));
                return false;
            }
        }

        public IAccount<BankAccount, BankAccount> GetAccountByIDAndType(string OwnerID, int accountType)
        {
            IAccount<BankAccount, BankAccount> account = _accountsStorage.GetAccountByID(OwnerID, accountType);
            if (account != null) return account;
            else return null;
        }

        private string SaveActionLog(string actionType, string accountNumber, string value)
        {
            StringBuilder line = new StringBuilder();

            line.AppendFormat(
                $"{actionType}\t" +
                $"{DateTime.Now}\t" +
                $"{Name}\t" +
                $"{accountNumber}\t" +
                $"{value}");
            return line.ToString();
        }
    }
}
