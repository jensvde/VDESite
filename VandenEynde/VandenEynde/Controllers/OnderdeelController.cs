using System.Collections.Generic;
using System.Linq;
using BL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VandenEynde.Models;

namespace VandenEynde.Controllers
{
    [Authorize]
    public class OnderdeelController : Controller
    {
        private IAutoManager mgr;

        public OnderdeelController()
        {
            mgr = new AutoManager();
        }
        // GET
        public IActionResult Index()
        {
            
            return View(mgr.GetOnderdelen());
        }
        
        [HttpPost]
        public IActionResult Index(int id)
        {
            
            return View(mgr.GetOnderdelen().AsEnumerable().Where(x => x.Auto.AutoId == id));
        }

        public IActionResult Details(int id)
        {
            
            return View(mgr.GetOnderdelen().ToList().Find(x => x.OnderdeelId == id));
        }

        public IActionResult New(int id)
        {
            Onderdeel onderdeel = new Onderdeel();
            onderdeel.Auto = mgr.GetAuto(id);
            return View(onderdeel);
        }
        [HttpPost]
        public IActionResult New(Onderdeel onderdeel , int id)
        {
            
            mgr.GetAuto(id).Onderdelen.Add(onderdeel);
            Onderdeel added = mgr.AddOnderdeel(onderdeel);
            return RedirectToAction("Details","Onderdeel",new {id = added.OnderdeelId});
        }
        [HttpPost]

        public IActionResult Edit(Onderdeel onderdeel)
        {
            mgr.ChangeOnderdeel(onderdeel);
            return RedirectToAction("Details", "Onderdeel", new {id = onderdeel.OnderdeelId});
        }

        public IActionResult Delete(int id)
        {
            
            mgr.DeleteOnderdeel(mgr.GetOnderdeel(id));
            return RedirectToAction("Index", "Onderdeel");
        }

        public IActionResult Add()
        {
            AutosOnderdeel autosOnderdeel = new AutosOnderdeel
            {
                Items = new List<SelectListItem>(), Onderdeel = new Onderdeel()
            };
            foreach (Auto auto in mgr.GetAutos())
            {
                SelectListItem item = new SelectListItem(value: auto.AutoId+"", text:auto.Naam);
                autosOnderdeel.Items.Add(item);
            }
            return View(autosOnderdeel);
        }

        [HttpPost]
        public IActionResult Add(AutosOnderdeel autosOnderdeel)
        {
            if (ModelState.IsValid)
            {
                mgr.GetAuto(autosOnderdeel.id).Onderdelen.Add(autosOnderdeel.Onderdeel);
                Onderdeel added = mgr.AddOnderdeel(autosOnderdeel.Onderdeel);
                return RedirectToAction("Details","Onderdeel",new {id = added.OnderdeelId});
            }

            return View();
        }

    }
}