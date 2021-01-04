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
        public string SearchString { get; set; }

        public IList<OnderdeelModel> Merken { get; set; } 
        public OnderdeelModel[] GelesecteerdeMerken { get; set; }
        public OnderdeelIndexModel()
        {
            this.Onderdelen = new List<Onderdeel>();
            this.Autos = new string[1000];
            this.Bestelnummers = new string[1000];
            this.Merken = new List<OnderdeelModel>();
            this.GelesecteerdeMerken = new OnderdeelModel[1000];

        }
    }
}
