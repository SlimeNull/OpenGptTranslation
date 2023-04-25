using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBarbecueGrill.Utilities
{
    internal static class StringUtils
    {
        public static int Count(this string str, string substring)
        {
            int count = 0;
            int i = 0;

            while ((i = str.IndexOf(substring, i)) != -1)
            {
                i += substring.Length;
                count++;
            }

            return count;
        }

        public static int IndexOfSkip(this string str, string substring, int count)
        {
            int i = 0;

            while (count > 0)
            {
                i = str.IndexOf(substring, i);

                if (i == -1)
                    return -1;

                i += substring.Length;
                count--;
            }

            return i;
        }

        public static string NormalizeLineEnding(this string origin)
        {
            StringBuilder sb = new StringBuilder(origin);
            sb.Replace("\r\n", "\n");
            sb.Replace("\r", "\n");

            return sb.ToString();
        }
    }
}
