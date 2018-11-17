using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSynApp.App.Models
{
    public class ISynReportResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ISynReport Report { get; set; }
    }
}
