using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace VandenEynde.Models
{
    public class AutosWerk
    {
        public List<SelectListItem> Items { get; set; }
        public int id { get; set; }
        public Werk Werk { get; set; }
    }
}