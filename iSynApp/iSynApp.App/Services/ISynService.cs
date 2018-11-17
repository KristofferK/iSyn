using iSynApp.App.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static iSynApp.App.Util.Util;

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
                    Tenancy = InBetween(source, "<div id=\"mainHeader\">Lejemål: ", "</div>"),
                    IsClosed = source.Contains("list-is-closed-date"),
                    BlueprintUrl = GetBlueprintUrl(source),
                    Landlord = GetLandlord(source),
                    Tenant = GetTenant(source)
                };
            }
        }

        private string GetBlueprintUrl(string source)
        {
            var url = InBetween(source, "\"filePath\": \"/files", "\"");
            if (url == null)
            {
                return null;
            }
            return "http://isynv1.dk/" + url;
        }

        private Landlord GetLandlord(string source)
        {
            source = InBetween(source, "<div class=\"text\">", "</div>", 1, 0);
            return new Landlord
            {
                Name = InBetween(source, "<span>", "</span>"),
                Department = InBetween(source, "<span>Afdeling&nbsp;", "</span>"),
                ApartmentId = InBetween(source, "<span>Lejemålsnr.: ", "</span>"),
                Address = InBetween(source, "<span>", "</span>", 4, 0),
                ZipCity = InBetween(source, "<span>", "</span>", 5, 0),
            };
        }

        private Tenant GetTenant(string source)
        {
            source = InBetween(source, "<div class=\"text\">", "</div>", 2, 0);
            return new Tenant
            {
                Name = InBetween(source, "<span>", "</span>"),
                Email = InBetween(source, "<span>E-mail: ", "</span>"),
                PhoneNo = InBetween(source, "<span>Telefon: ", "</span>"),
                OccupancyDate = InBetween(source, "<span>Indflytningsdato: ", "</span>")
            };
        }
    }
}
