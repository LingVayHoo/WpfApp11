using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task14_Library
{
    public static class MyExtensions
    {
        public static string ShortString(this string data)
        {
            if (data.Length > 10) { data = data.Substring(0, 10); }
            return data;
        }
    }
}
