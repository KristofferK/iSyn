using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iSynApp.App.Models
{
    public class Room
    {
        public string Title { get; set; }
        public IEnumerable<Deficiency> Deficiencies { get; set; }
    }
}
