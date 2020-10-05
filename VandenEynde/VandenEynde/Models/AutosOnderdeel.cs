using System.Collections.Generic;
using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VandenEynde.Models
{
    public class AutosOnderdeel
    {
        public List<SelectListItem> Items { get; set; }
        public int id { get; set; }
        public Onderdeel Onderdeel { get; set; }
    }
}