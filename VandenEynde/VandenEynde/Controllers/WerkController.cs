using System;
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
    public class WerkController : Controller
    {
        private IAutoManager mgr;

        public WerkController()
        {
            mgr = new AutoManager();
        }
        // GET
        public IActionResult Index()
        {
            return View(mgr.GetWerken());
        }
        
        [HttpPost]
        public IActionResult Index(int id)
        {
            return View(mgr.GetWerken().AsEnumerable().Where(x => x.Auto.AutoId == id));
        }
        
        public IActionResult Details(int id)
        {
            return View(mgr.GetWerken().ToList().Find(x => x.WerkId == id));
        }
        [HttpPost]

        public IActionResult Edit(Werk werk)
        {
            mgr.ChangeWerk(werk);
            return RedirectToAction("Index","Werk");
        }

        public IActionResult Add()
        {
            AutosWerk autosWerk = new AutosWerk()
            {
                Items = new List<SelectListItem>(), Werk = new Werk()
            };
            autosWerk.Werk.Datum = DateTime.Now;
            foreach (Auto auto in mgr.GetAutos())
            {
                SelectListItem item = new SelectListItem(value: auto.AutoId+"", text:auto.Naam);
                autosWerk.Items.Add(item);
            }
            return View(autosWerk);
        }
        [HttpPost]
        public IActionResult Add(AutosWerk autosWerk)
        {
            if (ModelState.IsValid)
            {
                mgr.GetAuto(autosWerk.id).WerkVoorAuto.Add(autosWerk.Werk);
                Werk added = mgr.AddWerk(autosWerk.Werk);
                return RedirectToAction("Details","Werk",new {id = added.WerkId});
            }

            return View();
        }
        
        public IActionResult Delete(int id)
        {
            
            mgr.DeleteWerk(mgr.GetWerk(id));
            return RedirectToAction("Index", "Werk");
        }
        
        public IActionResult New(int id)
        {
            Werk werk = new Werk();
            werk.Datum = DateTime.Now;
            werk.Auto = mgr.GetAuto(id);
            return View(werk);
        }
        [HttpPost]
        public IActionResult New(Werk werk , int id)
        {
            
            mgr.GetAuto(id).WerkVoorAuto.Add(werk);
            Werk added = mgr.AddWerk(werk);
            return RedirectToAction("Details","Werk",new {id = added.WerkId});
        }
    }
}