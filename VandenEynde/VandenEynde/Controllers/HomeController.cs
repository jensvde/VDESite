using BL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using VandenEynde.Models;

namespace VandenEynde.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IAutoManager mgr;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            mgr = new AutoManager();
        }

        public IActionResult Index()
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            foreach (Auto auto in mgr.GetAutos())
            {
                SelectListItem item = new SelectListItem
                {
                    Value = "" + auto.AutoId,
                    Text = auto.Naam
                };
                Items.Add(item);
            }
            HomeModel model = new HomeModel
            {
                Items = Items
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
