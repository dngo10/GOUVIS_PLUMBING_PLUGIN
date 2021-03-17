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
        public static string ConvertToFractionalNumber(double number)
        {
            if(number == ConstantName.invalid)
            {
                return "";
            }
            string numberStr = number.ToString();

            string realNumberEx = @"^([-+]?)([0-9]*)\.?([0-9]+)$";
            Regex rx = new Regex(realNumberEx);
            Match match = rx.Match(numberStr);

            int dotIndex = numberStr.IndexOf('.');
            string group1 = match.Groups[1].Value;
            string group2 = match.Groups[2].Value;
            string group3 = match.Groups[3].Value;

            if(dotIndex == -1)
            {
                return group3;
            }

            if(group2 == "0")
            {
                group2 = "";
            }
            
            if(group3 != "")
            {
                group3 = ConvertDenominatorToFractionalNumber(group3);
            }

            if(group3 != "" && group2 != "")
            {
                group2 = group2 + "-";
            }

            return group1 + group2 + group3;
        }

        public static string ConvertDenominatorToFractionalNumber(string numStr)
        {
            int number;
            if (!int.TryParse(numStr, out number)) return "";

            int denominator = (int)Math.Pow(10, numStr.Length);

            int commonDivisor = FindLargestDivisor(number, denominator);

            int newNumerator = number / commonDivisor;
            int newDenominator = denominator / commonDivisor;

            return newNumerator.ToString() + "/" + newDenominator.ToString();
        }

        public static int FindLargestDivisor(int a, int b)
        {
            return _Gcd(a, b);
        }

        public static int _Gcd(int a, int b)
        {
            return b == 0 ? a : _Gcd(b, a % b);
        }
    }
}
