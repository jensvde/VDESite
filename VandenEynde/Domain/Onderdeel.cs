using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Onderdeel
    {
        [Key]
        public int OnderdeelId { get; set; }
        
        public virtual Auto Auto { get; set; }
        public string Beschrijving { get; set; }
        public string Merk { get; set; }
        public string BestelNr { get; set; }
    }
}