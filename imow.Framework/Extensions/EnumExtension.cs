using System;

namespace Imow.Framework.Extensions
{
    public static class EnumExtension
    {
        public static int ToInt(this Enum self)
        {
            return int.Parse(self.ToString("D"));
        }

        public static char ToChar(this Enum self)
        {
            return char.Parse(self.ToString("D"));
        }

        public static string ToStr(this Enum self)
        {
            return self.ToString("D");
        }

    }
}