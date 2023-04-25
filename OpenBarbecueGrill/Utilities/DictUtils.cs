using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBarbecueGrill.Utilities
{
    static class DictUtils
    {
        public static void Populate<TKey, TValue>(this Dictionary<TKey, TValue> origin, Dictionary<TKey, TValue> another) where TKey : notnull
        {
            foreach (var kv in another)
                origin[kv.Key] = kv.Value;
        }
    }
}
