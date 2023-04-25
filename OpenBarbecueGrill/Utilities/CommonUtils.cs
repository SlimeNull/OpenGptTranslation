using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBarbecueGrill.Utilities
{
    static class CommonUtils
    {
        public static Uri GetAssemblyResourceUri(string absPath)
        {
            if (!absPath.StartsWith('/'))
                absPath = $"/{absPath}";

            return new Uri($"pack://aplication:,,,/OpenBarbecueGrill;component{absPath}");
        }
    }
}
