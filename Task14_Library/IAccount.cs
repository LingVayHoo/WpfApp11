using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task14_Library
{
    public interface IAccount<in K, out T>
    {
        K SetValue { set; }
        void SetFields(params string[] data);
        void SetValueMethod(string[] data);

        T GetValue { get; }
        T GetValueMethod();
        //void SetAccountType();

    }
}
