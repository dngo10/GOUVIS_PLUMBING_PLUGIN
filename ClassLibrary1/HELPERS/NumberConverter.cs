using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassLibrary1.HELPERS
{
    class NumberConverter
    {
        public string ConvertToRationalNumber(double number)
        {
            string numberStr = number.ToString();

            string realNumberEx = @"^([-+]?[0-9]*)\.?([0-9]+)$";
            Regex rx = new Regex(realNumberEx);
            Match match = rx.Match(numberStr);
            int dotIndex = numberStr.IndexOf('.');

            if (dotIndex == 0)
            {
                string Grounp2 = match.Groups[2].Value;
            }

            return ;
        }

        public static int FindLargestDivider(int a, int b)
        {
            return _Gcd(a, b);
        }

        public static int _Gcd(int a, int b)
        {
            return b == 0 ? a : _Gcd(b, a % b);
        }
    }
}
