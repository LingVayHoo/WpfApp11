using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp11
{
    public class BankAccount
    {
        protected List<string> _usedNumbers;

        protected AccountsStorage _accountsStorage = new AccountsStorage();
        protected string _ownerID;
        protected string _accountNumber;
        protected float _accountBalance;
        protected DateTime _createDate;
        protected int _accountTypeInt;
        protected int _persents;

        public string OwnerID
        { 
            get { return _ownerID; } 
            set { _ownerID = value; } 
        }

        public string AccountNumber
        { 
            get { return _accountNumber; } 
            set { _accountNumber = value; } 
        }

        public float AccountBalance
        { 
            get { return _accountBalance; }
            set { _accountBalance = value; }
        }

        public DateTime CreateDate
        { 
            get { return _createDate; }
            set { _createDate = value; } 
        }

        public int AccountTypeInt
        {
            get { return _accountTypeInt; }
            set { _accountTypeInt = value; }
        }

        public int Persents
        {
            get { return _persents; }
            set { _persents = value; }
        }

        public BankAccount() : this(String.Empty)
        {

        }

        public BankAccount(string ownerID)
        {
            OwnerID = ownerID;
            GenerateAccountNumber();
            AccountBalance = 0;
            CreateDate = DateTime.Now;
            
            Persents = 0;
        }

        public BankAccount(string[] data)
        {
            //SetAccountType();
            OwnerID = data[0];
            AccountNumber = data[1];
            if (float.TryParse(data[2], out float accResult)) AccountBalance = accResult;
            if (DateTime.TryParse(data[3], out DateTime dateResult)) CreateDate = dateResult;
            if (int.TryParse(data[4], out int typeResult)) AccountTypeInt = typeResult;
        }

        public override string ToString()
        {
            StringBuilder line = new StringBuilder();

            line.AppendFormat(
                $"{OwnerID}#" +
                $"{AccountNumber}#" +
                $"{AccountBalance}#" +
                $"{CreateDate}#" +
                $"{AccountTypeInt}");
            return line.ToString();
        }

        public void Save()
        {
            string[] data = ToString().Split('#');
            _accountsStorage.SaveAccount(data);
        }

        public void Delete()
        {
            _accountsStorage.DeleteAccount(this);
        }

        /// <summary>
        /// Метод изменения баланса с защитой от отрицательных значений
        /// </summary>
        /// <param name="changeValue"> должно быть отрицательным, если требуется уменьшить баланс</param>
        /// <returns>true, если на счете хавтает средств</returns>
        public bool ChangeBalance(float changeValue)
        {
            if (AccountBalance + changeValue >= 0)
            {
                AccountBalance += changeValue;
                return true;
            }
            else
            {
                return false;
            }
        }

        protected string GenerateAccountNumber()
        {            
            RefreshData();
            return SetNewAccountNumber();
        }


        /// <summary>
        /// Рекурсия для генерации уникального номера
        /// </summary>
        /// <returns></returns>
        protected string SetNewAccountNumber()
        {
            Random rnd = new Random();
            AccountNumber = rnd.Next(10000, 100000).ToString();
            if (_usedNumbers.Contains(AccountNumber)) 
                return SetNewAccountNumber();
            else 
                return AccountNumber;
        }        

        protected void RefreshData()
        {
            _usedNumbers = _accountsStorage.GetAccountNumbers();
        }

    }
}
