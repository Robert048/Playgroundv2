using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground_v2
{
    public class dbObject
    {
        public string naam { get; set; }

        public dbObject(string naam)
        {
            this.naam = naam;
        }
    }
}
