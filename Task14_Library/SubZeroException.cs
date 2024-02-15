using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task14_Library
{
    public class SubZeroException : Exception
    {

        private string exceptionMessage;

        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            private set { exceptionMessage = value; }
        }

        public override string Message => ExceptionMessage;

        public SubZeroException()
        {
            ExceptionMessage = "Значение не может быть отрицательным!";
        }
    }
}
