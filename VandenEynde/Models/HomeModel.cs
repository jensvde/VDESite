using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace VandenEynde.Models
{
    public class HomeModel
    {
        public List<SelectListItem> Items { get; set; }
        public int Id { get; set; }
    }
}