using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task14_Library
{
    public class PersonDataStorage
    {
        private string _fileName = "PersonData.txt";

        public List<Person> GetDataBase()
        {
            List<Person> persons = new List<Person>();
            if (IsFileExists())
            {
                using (StreamReader sr = new StreamReader(_fileName, Encoding.Unicode))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        persons.Add(new Person(line));
                    }
                }
            }
            return persons;
        }

        public Person GetPersonByID(string id)
        {
            List<Person> persons = GetDataBase();
            Person person = null;
            foreach (Person p in persons)
            {
                if (p.ID == id)
                {
                    person = p;
                    break;
                }
            }
            return person;
        }

        public void SavePerson(Person person)
        {
            List<Person> persons = GetDataBase();
            if (IsPersonExists(persons, person)) persons.Remove(person);
            persons.Add((person));
            SaveDataBase(persons);
        }

        public void DeletePerson(Person person)
        {
            List<Person> persons = GetDataBase();
            if (IsPersonExists(persons, person)) persons.Remove(person);
            SaveDataBase(persons);
        }

        public List<string> GetIDs()
        {
            List<Person> persons = GetDataBase();
            List<string> ids = new List<string>();

            foreach (Person person in persons)
            {
                ids.Add(person.ID);
            }
            return ids;
        }

        private bool IsPersonExists(List<Person> persons, Person person)
        {
            bool temp = false;
            foreach (Person p in persons)
            {
                if (p.ID == person.ID)
                {
                    temp = true;
                    break;
                }
            }
            return temp;
        }

        private void SaveDataBase(List<Person> persons)
        {

            using (StreamWriter sw = new StreamWriter(_fileName, false, Encoding.Unicode))
            {
                foreach (Person person in persons)
                {
                    sw.WriteLine(person.ToString());
                }
            }

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
