using iSynApp.App.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace iSynApp.App.Services
{
    public class ISynService
    {
        public Task<ISynReport> GetReportAsync(string code)
        {
            return Task.FromResult(new ISynReport
            {
                Code = code,
                Tenancy = "02-10-4112-14",
                BlueprintUrl = "http://isynv1.dk/files/blueprints/14/32/2-10-4112_29121.jpg"
            });
        }
    }
}
