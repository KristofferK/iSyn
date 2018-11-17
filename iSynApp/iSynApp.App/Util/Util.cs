using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSynApp.App.Util
{
    public static class Util
    {
        public static string InBetween(string haystack, string afterThis, string beforeThis)
        {
            return InBetween(haystack, afterThis, beforeThis, 1, 0);
        }

        public static string InBetween(string haystack, string afterThis, string beforeThis,
            int afterIndex, int beforeIndex, bool includeAfterAndBefore = false)
        {
            if (haystack == null) return null;

            if (haystack.Contains(afterThis))
            {
                var split = Split(haystack, afterThis);
                if (split.Length > afterIndex)
                {
                    string rv = split[afterIndex];
                    if (rv.Contains(beforeThis))
                    {
                        split = Split(rv, beforeThis);
                        if (split.Length > beforeIndex)
                        {
                            rv = split[beforeIndex];

                            if (includeAfterAndBefore)
                            {
                                rv = $"{afterThis}{rv}{beforeThis}";
                            }

                            return rv;
                        }
                    }
                }
            }
            return null;
        }

        public static string[] Split(string s, string separator)
        {
            return s.Split(new string[] { separator }, StringSplitOptions.None);
        }
    }
}
