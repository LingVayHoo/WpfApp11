//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Taasks;

//namespace WpfApp11
//{
//    public class ActionLog
//    {
//        private string _fileName = "ActionLog.csv";

//        public List<AccountAction> GetDataBase()
//        {
//            List<AccountAction> accountActions = new List<AccountAction>();
//            if (IsFileExists())
//            {
//                using (StreamReader sr = new StreamReader(_fileName, Encoding.Unicode))
//                {
//                    string line;

//                    while ((line = sr.ReadLine()) != null)
//                    {
//                        //accountActions.Add(new AccountAction(line));
//                    }
//                }
//            }
//            return accountActions;
//        }

//        public void SaveAction(AccountAction accountAction)
//        {
//            List<AccountAction> accountActions = GetDataBase();            
//            accountActions.Add((accountAction));
//            SaveDataBase(accountActions);
//        }

//        private void SaveDataBase(List<AccountAction> accountActions)
//        {

//            using (StreamWriter sw = new StreamWriter(_fileName, false, Encoding.Unicode))
//            {
//                foreach (AccountAction accountAction in accountActions)
//                {
//                    sw.WriteLine(accountAction.ToString());
//                }
//            }
//        }

//        public void SaveStringAction(string line)
//        {
//            using (StreamWriter sw = new StreamWriter(_fileName, true, Encoding.Unicode))
//            {
//                sw.WriteLine(line);
//            }
//        }

//        /// <summary>
//        /// Метод для проверки существования файла
//        /// </summary>
//        /// <returns></returns>
//        private bool IsFileExists()
//        {
//            FileInfo file = new FileInfo(_fileName);
//            return file.Exists;
//        }
//    }
//}
