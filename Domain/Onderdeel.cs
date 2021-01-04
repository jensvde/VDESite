using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Onderdeel
    {
        [Key]
        public int OnderdeelId { get; set; }

        public string Beschrijving { get; set; }
        public string Merk { get; set; }
        public virtual ICollection<OnderdeelBestelnummer> Bestelnummers { get; set; }
        public virtual ICollection<AutoOnderdeel> AutoOnderdelen { get; set; }

    }
}