using System;
using System.Collections.Generic;
using System.Text;

namespace Task14_Library
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
            set { _name = value; }
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

            IAccount<BankAccount, BankAccount> account = _accountsStorage.GetAccount(accountNumber);
            // Выявляю тип аккаунта и присваиваю переменной другой тип для перевода денег
            // с закрываемого
            int temp = account.GetValue.AccountTypeInt > 0 ? 0 : 1;
            string text = "Счет удален!";
            if (TrySafeCredits(account.GetValue.OwnerID, temp, account.GetValue.AccountBalance))
            {
                text = "Счет удален, средства \nперечислены на другой счет!";
            }

            OnActionHappened?.Invoke(text);
            _actionLog.SaveStringAction(SaveActionLog(text, "-", "-"));
            account?.GetValue.Delete();
        }

        public bool CashTransfer(string outAccountNumber, string inAccountNumber, float creditsValue)
        {
            try
            {
                if (creditsValue < 0) throw new SubZeroException();
                if (outAccountNumber != String.Empty &&
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
                else if (outAccountNumber == String.Empty)
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
            catch (SubZeroException)
            {
                OnActionHappened("Неверная сумма транзакции!");
                return false;
            }

        }

        public IAccount<BankAccount, BankAccount> GetAccountByIDAndType(string OwnerID, int accountType)
        {
            IAccount<BankAccount, BankAccount> account = _accountsStorage.GetAccountByID(OwnerID, accountType);
            if (account != null) return account;
            else return null;
        }

        public bool TrySafeCredits(string ownerID, int accountType, float creditsValue)
        {
            IAccount<BankAccount, BankAccount> account = GetAccountByIDAndType(ownerID, accountType);

            if (account != null)
            {
                if (CashTransfer(String.Empty, account.GetValue.AccountNumber, creditsValue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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
