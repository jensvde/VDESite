using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VandenEynde.Models
{
    public class OnderdeelIndexModel
    {
        public IList<Onderdeel> Onderdelen { get; set; }
        public string[] Autos { get; set; }
        public string[] Bestelnummers { get; set; }


        public OnderdeelIndexModel()
        {
            this.Onderdelen = new List<Onderdeel>();
            this.Autos = new string[1000];
            this.Bestelnummers = new string[1000];
        }
    }
}
