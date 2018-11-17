using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSynApp.App.Models
{
    public class Tenant
    {
        public string Name { get; internal set; }
        public string Email { get; internal set; }
        public string PhoneNo { get; internal set; }
        public string OccupancyDate { get; internal set; }
    }
}
