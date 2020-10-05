using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VandenEynde.Models
{
    public class HomeModel
    {
        public List<SelectListItem> Items { get; set; }
        public int Id { get; set; }
    }
}