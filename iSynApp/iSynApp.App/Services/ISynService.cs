using iSynApp.App.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace iSynApp.App.Services
{
    public class ISynService
    {
        public Task<ISynReport> GetReportAsync(string code)
        {
            return Task.FromResult(Scrape(code));
        }

        private ISynReport Scrape(string code)
        {
            var url = "http://isynv1.dk/deficiency.asp?code=" + code;
            using (WebClient wc = new WebClient())
            {
                var source = wc.DownloadString(url);
                if (source.Length < 100)
                {
                    return null;
                }

                return new ISynReport()
                {
                    Code = code,
                    IsClosed = source.Contains("list-is-closed-date"),
                    Tenancy = InBetween(source, "<div id=\"mainHeader\">Lejemål: ", "</div>"),
                };
            }
        }

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
