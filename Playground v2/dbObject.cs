using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground_v2
{
    //DataBase Object class
    public class dbObject
    {
        //TODO fields from database
        public string naam { get; set; }

        public dbObject(string naam)
        {
            this.naam = naam;
        }
    }
}
