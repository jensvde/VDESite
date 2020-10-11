using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace VandenEynde.Models
{
    public class AutoModel
    {
        public Auto Auto { get; set; }

        public IList<Onderdeel> OnderdelenVoorAuto { get; set; }
    }
}