using BL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Werk werkToChange = mgr.GetWerk(werk.WerkId);
            if(werk.Extra != null) { werkToChange.Extra = werk.Extra; }
            if (werk.Datum != null) { werkToChange.Datum = werk.Datum; }
            if (werk.KilometerStand != werkToChange.KilometerStand && werk.KilometerStand > 0) { werkToChange.KilometerStand = werk.KilometerStand; }
            if (werk.OliefilterVervangen != werkToChange.OliefilterVervangen) { werkToChange.OliefilterVervangen = werk.OliefilterVervangen; }
            if (werk.LuchtfilterVervangen != werkToChange.LuchtfilterVervangen) { werkToChange.LuchtfilterVervangen = werk.LuchtfilterVervangen; }
            if (werk.BrandstoffilterVervangen != werkToChange.BrandstoffilterVervangen) { werkToChange.BrandstoffilterVervangen = werk.BrandstoffilterVervangen; }
            if (werk.InterieurfilterVervangen != werkToChange.InterieurfilterVervangen) { werkToChange.InterieurfilterVervangen = werk.InterieurfilterVervangen; }
            mgr.ChangeWerk(werkToChange);
            return RedirectToAction("Details", "Werk", new { id = werkToChange.WerkId});
        }

        public IActionResult Add()
        {
            AutosWerk autosWerk = new AutosWerk()
            {
                Items = new List<SelectListItem>(),
                Werk = new Werk()
            };
            autosWerk.Werk.Datum = DateTime.Now;
            foreach (Auto auto in mgr.GetAutos())
            {
                SelectListItem item = new SelectListItem(value: auto.AutoId + "", text: auto.Naam);
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
                return RedirectToAction("Details", "Werk", new { id = added.WerkId });
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
            Auto auto = mgr.GetAuto(id);
            Werk previousWerk = null;
            if(auto.WerkVoorAuto != null)
            {
                if(auto.WerkVoorAuto.Count > 1) 
                { 
                    previousWerk = mgr.GetWerk(id--);
                }  
            }
            
            Werk werk = new Werk
            {
                Datum = DateTime.Now,
                Auto = auto
            };

            if (previousWerk != null)
            {
                werk.KilometerStand = previousWerk.KilometerStand;
            }
            return View(werk);
        }
        [HttpPost]
        public IActionResult New(Werk werk, int id)
        {

            mgr.GetAuto(id).WerkVoorAuto.Add(werk);
            Werk added = mgr.AddWerk(werk);
            return RedirectToAction("Details", "Werk", new { id = added.WerkId });
        }
    }
}