using iSynApp.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static iSynApp.App.Util.Util;

namespace iSynApp.App.Services
{
    public class ISynService
    {
        public Task<ISynReportResult> GetReportAsync(string code)
        {
            if (!Regex.IsMatch(code, "^[a-zA-Z0-9]{20}$"))
            {
                return Task.FromResult(new ISynReportResult
                {
                    Report = null,
                    Message = "Rapportnummeret skal bestå af bogstaverne a-z, A-Z samt 0-9, og skal være 20 tegn langt",
                    Success = false,
                });
            }

            var report = Scrape(code);
            if (report == null)
            {
                return Task.FromResult(new ISynReportResult
                {
                    Report = null,
                    Message = "Der blev ikke fundet en rapport med det angivne nummer",
                    Success = false,
                });
            }

            return Task.FromResult(new ISynReportResult
            {
                Report = report,
                Message = null,
                Success = true,
            });
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
                    Tenant = GetTenant(source),
                    Rooms = GetRoomsWithDeficiencies(source)
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
            return "http://isynv1.dk/files" + url;
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

        private IEnumerable<Room> GetRoomsWithDeficiencies(string source)
        {
            var rooms = new Dictionary<string, IList<Deficiency>>();
            Split(source, "<div class=\"info\">")
                .Skip(1)
                .Select(e => Split(e, "</div>")[0])
                .ToList()
                .ForEach(e =>
                {
                    var location = InBetween(e, "<h5>", "</h5>");
                    var description = WebUtility.HtmlDecode(InBetween(e, "<p>", "</p>")).Replace("<br>", " ");
                    description = Regex.Replace(description, "<[^>]+>", "");
                    description = description.Trim();
                    description = description[0].ToString().ToUpper() + description.Substring(1);

                    if (!rooms.ContainsKey(location))
                    {
                        rooms[location] = new List<Deficiency>();
                    }
                    rooms[location].Add(new Deficiency(description));
                });
            return rooms.Select(e => new Room
            {
                Title = e.Key,
                Deficiencies = e.Value.OrderBy(deficiency => deficiency.Description)
            })
            .OrderBy(e => e.Title);
        }
    }
}
