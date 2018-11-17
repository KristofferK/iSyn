using System;

namespace iSynApp.App.Models
{
    public class ISynReport
    {
        public string Code { get; set; }
        public string Tenancy { get; set; }
        public bool IsClosed { get; set; }
        public string BlueprintUrl { get; set; }
        public Tenant Tenant { get; set; }
        public Landlord Landlord { get; set; }
    }
}
