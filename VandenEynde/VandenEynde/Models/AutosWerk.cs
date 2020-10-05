using System.Collections.Generic;
using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VandenEynde.Models
{
    public class AutosWerk
    {
        public List<SelectListItem> Items { get; set; }
        public int id { get; set; }
        public Werk Werk { get; set; }
    }
}