using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Werk
    {
        [Key]
        public int WerkId { get; set; }
        public virtual Auto Auto { get; set; }
        public int KilometerStand { get; set; }
        public bool OliefilterVervangen { get; set; }
        public bool LuchtfilterVervangen { get; set; }
        public bool BrandstoffilterVervangen { get; set; }
        public bool InterieurfilterVervangen { get; set; }
        public string Extra { get; set; }
        public DateTime Datum { get; set; }
    }
}