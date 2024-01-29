using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp11
{
    public class Person
    {
        private List<string> _usedIDs;

        private PersonDataStorage _personData = new PersonDataStorage();
        private string _id;
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private string _mobilePhone;
        private string _passportID;
                 
        //static Person()
        //{
        //    RefreshData();
        //}

        public string ID 
        { 
            get { return _id; } 
            private set { _id = value; }
        }

        public string FirstName
        { 
            get { return _firstName; } 
            set {  _firstName = value; } 
        }

        public string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; }
        }

        public string LastName 
        { 
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string MobilePhone
        {
            get { return _mobilePhone; }
            set { _mobilePhone = value; }
        }

        public string PassportID
        {
            get { return _passportID; }
            set { _passportID = value; }
        }

        /// <summary>
        /// Конструктор для создания базы данных из файла
        /// </summary>
        /// <param name="line"></param>
        public Person(string line)
        {
            string[] data = line.Split('#');
            if(data.Count() == 6)
            {
                ID = data[0];
                FirstName = data[1];
                MiddleName = data[2];
                LastName = data[3];
                MobilePhone = data[4];
                PassportID = data[5];
            }

        }

        /// <summary>
        /// Конструктор для создания нового класса
        /// </summary>
        /// <param name="data"></param>
        public Person(string[] data)
        {
            CreateID();
            FirstName = data[0];
            MiddleName = data[1];
            LastName = data[2];
            MobilePhone = data[3];
            PassportID = data[4];
        }

        public Person()
        {            
            FirstName = String.Empty;
            MiddleName = String.Empty;
            LastName = String.Empty;
            MobilePhone = String.Empty;
            PassportID = String.Empty;
        }

        public override string ToString()
        {
            StringBuilder line = new StringBuilder();

            line.AppendFormat(
                $"{ID}#" +
                $"{FirstName}#" +
                $"{MiddleName}#" +
                $"{LastName}#" +
                $"{MobilePhone}#" +
                $"{PassportID}");
            return line.ToString();
        }

        public void Save()
        {
            _personData.SavePerson(this);
        }

        public void Delete()
        {
            _personData.DeletePerson(this);
        }

        private void CreateID()
        {
            ID = $"{Guid.NewGuid().ToString().Substring(0, 5)}";
            RefreshData();
            Console.WriteLine("test");
            if (_usedIDs.Contains(ID)) CreateID();
            else _usedIDs.Add(ID);
        }

        private void RefreshData()
        {
            _usedIDs = _personData.GetIDs();
        }
    }
}
