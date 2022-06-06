using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Management;

namespace YourProgramName
{
    public static class Helper
    {
        /*
         * Add character to make specific length
         * string str = AddChar("21", '0', 4);
         * str will be "0021".
         */
        public static string AddChar(string s, char c, int target_length)
        {
            string res = "";
            for(int i=0; i< target_length - (s.Length); i++)
            {
                res += c;
            }
            return res + s;
        }

        /* Convert myanmar unicode digit to ASCII digit */
        public static string MMN_ENN(string str)
        {
            str = str.Replace('၀', '0');
            str = str.Replace('၁', '1');
            str = str.Replace('၂', '2');
            str = str.Replace('၃', '3');
            str = str.Replace('၄', '4');
            str = str.Replace('၅', '5');
            str = str.Replace('၆', '6');
            str = str.Replace('၇', '7');
            str = str.Replace('၈', '8');
            str = str.Replace('၉', '9');

            return str;
        }

        /* Convert ASCII digit to myanmar unicode digit */
        public static string ENN_MMN(string str)
        {
            str = str.Replace('0', '၀');
            str = str.Replace('1', '၁');
            str = str.Replace('2', '၂');
            str = str.Replace('3', '၃');
            str = str.Replace('4', '၄');
            str = str.Replace('5', '၅');
            str = str.Replace('6', '၆');
            str = str.Replace('7', '၇');
            str = str.Replace('8', '၈');
            str = str.Replace('9', '၉');

            return str;
        }

        public static int GetINT(string str)
        {
            str = Regex.Replace(str, "[^0-9-]", "");
            return String.IsNullOrEmpty(str) ? 0 : Int32.Parse(str);
        }

        public static int EN_INT(string str)
        {
            str = MMN_ENN(str);
            return GetINT(str);
        }

        public static Double GetDouble(string str)
        {
            str = Regex.Replace(str, "[^0-9.]", "");
            return String.IsNullOrEmpty(str) ? 0 : Double.Parse(str);
        }

        /*
         * Convert Myanmar Number String to Double value
         * ၂၉၃၈၄၇၅ => 2938475
         */
        public static Double EN_DOUBLE(string str)
        {
            str = MMN_ENN(str);
            return GetDouble(str);
        }

        public static DateTime ToDateTime(this string s, string format = "ddMMyyyy", string cultureString = "tr-TR")
        {
            try
            {
                var r = DateTime.ParseExact(s: s, format: format, provider: CultureInfo.GetCultureInfo(cultureString));
                return r;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (CultureNotFoundException)
            {
                throw; // Given Culture is not supported culture
            }
        }

        public static DateTime ToDateTime(this string s, string format, CultureInfo culture)
        {
            try
            {
                var r = DateTime.ParseExact(s: s, format: format, provider: culture);
                return r;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (CultureNotFoundException)
            {
                throw; // Given Culture is not supported culture
            }

        }
        
        /* 
         * ဂဏန်းများကို စာအဖြစ်သို့ပြောင်းရန် 
         * ဥပမာ - 
         *  23000000  => ၂၃၀သိန်း
         *  2305012   => ၂၃သိန်း ၅ထောင် ၁ဆယ် ၂ကျပ်
         */
        public static string NumberToString(string value)
        {
            value = Helper.ENN_MMN(value);
            string s = "";
            var strArr = value.ToString().ToCharArray();

            string[] numberArr = new string[] { "ကျပ်", "ဆယ်", "ရာ", "ထောင်", "သောင်း", "သိန်း"};
            
            //9,1 2 3 4 5 6
            //    7-1 =6
            int strLen = strArr.Length - 1;
            int j = 0;
            string lakh = "";
            for (int i = strLen; i >= 0; i--)
            {
                if(j<5)
                {
                    if(strArr[i].ToString() == "၀")
                        j++;
                    else
                        s = strArr[i].ToString() + numberArr[j++] + " " + s;
                }
                else
                {
                    lakh = strArr[i].ToString() + lakh;
                }
            }
            if(strLen > 4)
            {
                s = lakh + numberArr[j]+ " " + s;
            }

            return s;
        }
    }
}
