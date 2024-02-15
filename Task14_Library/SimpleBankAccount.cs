using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task14_Library
{
    public class SimpleBankAccount : IAccount<BankAccount, BankAccount>
    {
        private BankAccount _bankAccount;

        public SimpleBankAccount(string[] data)
        {
            SetValueMethod(data);
        }

        public SimpleBankAccount(string data)
        {
            _bankAccount = new BankAccount(data);
        }

        public SimpleBankAccount()
        {
            _bankAccount = new BankAccount(String.Empty);
            ChangeAccountNumber();
        }

        public BankAccount GetValue
        {
            get { return _bankAccount; }
        }

        public BankAccount SetValue
        {
            set { _bankAccount = value; }
        }

        public void SetValueMethod(string[] data)
        {
            _bankAccount = new BankAccount(data);
        }

        public void SetFields(params string[] data)
        {
            if (_bankAccount == null) _bankAccount = new BankAccount();
            for (int i = 0; i < data.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        _bankAccount.OwnerID = data[i];
                        ChangeAccountNumber();
                        break;
                    case 1:
                        _bankAccount.AccountNumber = data[i];
                        break;
                    case 2:
                        if (float.TryParse(data[i], out float balance))
                            _bankAccount.AccountBalance = balance;
                        break;
                    case 3:
                        if (DateTime.TryParse(data[i], out DateTime createDate))
                            _bankAccount.CreateDate = createDate;
                        break;
                    default:
                        break;
                }
            }
        }

        public BankAccount GetValueMethod()
        {
            return _bankAccount;
        }

        private void ChangeAccountNumber()
        {
            _bankAccount.AccountTypeInt = 0;
        }


    }
}
