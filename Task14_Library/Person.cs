using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task14_Library
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
            set { _firstName = value; }
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
            if (data.Count() == 6)
            {
                ID = data[0];
                data[1].ShortString();
                FirstName = data[1];
                data[2].ShortString();
                MiddleName = data[2];
                data[3].ShortString();
                LastName = data[3];
                LastName.ShortString();
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
            FirstName = data[0].ShortString();
            MiddleName = data[1].ShortString();
            LastName = data[2].ShortString();
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
            if (_usedIDs.Contains(ID)) CreateID();
            else _usedIDs.Add(ID);
        }

        private void RefreshData()
        {
            _usedIDs = _personData.GetIDs();
        }
    }
}
