using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task14_Library
{
    public class AccountAction
    {
        public string ActionType { get; set; }
        public string TimeOfAction { get; set; }
        public string ActionOwner { get; set; }
        public string AccountNumber { get; set; }
        public string Value { get; set; }

        public AccountAction(string line)
        {
            string[] data = line.Split('#');
            ActionType = data[0];
            TimeOfAction = data[1];
            ActionOwner = data[2];
            AccountNumber = data[3];
            Value = data[4];
        }

        public override string ToString()
        {
            StringBuilder line = new StringBuilder();

            line.AppendFormat(
                $"{ActionType}\t" +
                $"{TimeOfAction}\t" +
                $"{ActionOwner}\t" +
                $"{AccountNumber}" +
                $"{Value}");
            return line.ToString();
        }
    }
}
