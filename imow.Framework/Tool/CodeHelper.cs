using System;

namespace imow.Framework.Tool
{
    public enum CodeLetterType
    {
        Number = 1,
        Letter = 2,
        All = 4
    }

    public class CodeHelper
    {
        /// <summary>
        /// 生成CODE
        /// </summary>
        public static string CreateCode(CodeLetterType type, int length)
        {
            return CreateCode(new CodeLetterType[] { type }, new int[] { length });
        }

        /// <summary>
        /// 生成CODE
        /// </summary>
        public static string CreateCode(CodeLetterType type, params int[] length)
        {
            string code = "";
            foreach (int len in length)
            {
                if (len <= 0)
                {
                    continue;
                }
                string cd = "";
                if (type == CodeLetterType.Number)
                {
                    cd = RandomNumber(len, true);
                }
                else if (type == CodeLetterType.Letter)
                {
                    cd = RandomStr_char(len, true);
                }
                else
                {
                    cd = RandomStr(len, true);
                }

                if (string.IsNullOrEmpty(code))
                {
                    code = cd;
                }
                else
                {
                    code = code + "-" + cd;
                }
            }
            return code;
        }

        /// <summary>
        /// 生成CODE
        /// </summary>
        public static string CreateCode(CodeLetterType[] type, int[] length)
        {
            string code = "";
            for (int i = 0; i < length.Length; i++)
            {
                int len = length[i];
                if (len <= 0)
                {
                    continue;
                }
                CodeLetterType t = CodeLetterType.All;
                if (type.Length > i)
                {
                    t = type[i];
                }

                string cd = "";
                if (t == CodeLetterType.Number)
                {
                    cd = RandomNumber(len, true);
                }
                else if (t == CodeLetterType.Letter)
                {
                    cd = RandomStr_char(len, true);
                }
                else
                {
                    cd = RandomStr(len, true);
                }

                if (string.IsNullOrEmpty(code))
                {
                    code = cd;
                }
                else
                {
                    code = code + "-" + cd;
                }

            }
            return code;
        }

        #region 随机数

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns></returns>
        private static string RandomNumber(int Length)
        {
            return RandomNumber(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        private static string RandomNumber(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }

        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="IntStr">生成长度</param>
        /// <returns></returns>
        private static string RandomStr(int Length)
        {
            return RandomStr(Length, false);
        }

        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        private static string RandomStr(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }

        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="IntStr">生成长度</param>
        /// <returns></returns>
        private static string RandomStr_char(int Length)
        {
            return RandomStr_char(Length, false);
        }

        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        private static string RandomStr_char(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }

        #endregion
    }
}
