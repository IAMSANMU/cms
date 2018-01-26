using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imow.Framework.Tool
{
    public class RandomHelper
    {
        public static int Next(int minvalue,int maxvalue)
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(minvalue, maxvalue);
        }


        public static String GetRandomCode(int length, String[] nots)
        {
            String checkCode = String.Empty;
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                lab:
                Int32 number = random.Next();
                Char code;
                if (number % 2 == 0)
                {
                    code = (Char)('0' + (Char)(number % 10));
                }
                else
                {
                    code = (Char)('A' + (Char)(number % 26));
                }
                //过滤不需要的字
                if (Array.IndexOf<String>(nots, code.ToString()) > -1)
                {
                    goto lab;
                }
                checkCode += code.ToString();
            }
            return checkCode;
        }
    }
}
