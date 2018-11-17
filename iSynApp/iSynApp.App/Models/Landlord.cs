using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSynApp.App.Models
{
    public class Landlord
    {
        public string Name { get; internal set; }
        public string Department { get; internal set; }
        public string ApartmentId { get; internal set; }
        public string Address { get; internal set; }
        public string ZipCity { get; internal set; }
    }
}
