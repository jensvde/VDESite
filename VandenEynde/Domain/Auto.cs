using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Auto
    {
        [Key]
        public int AutoId { get; set; }
        public string Naam { get; set; }
        public double OlieInhoud { get; set; }
        public int Bouwjaar { get; set; }
        public double CylinderInhoud { get; set; }
        public double VermogenKw { get; set; }
        public string MotorCode { get; set; }
        public string Banden { get; set; }
        public DateTime DatumAangemaakt { get; set; }
        
        public virtual ICollection<Werk> WerkVoorAuto { get; set; }
        public virtual ICollection<Onderdeel> Onderdelen { get; set; } 
    }
}