using BL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VandenEynde.Models;

namespace VandenEynde.Controllers
{
     [Authorize]
    public class AutoController : Controller
    {
        private IAutoManager mgr;

        public AutoController()
        {
            mgr = new AutoManager();
        }

        [HttpPost]
        public IActionResult Edit(Auto auto)
        {
            mgr.ChangeAuto(auto);
            return RedirectToAction("Details", "Auto", new { id = auto.AutoId }); ;
        }
        public IActionResult Delete(int id)
        {
            Auto toRemove = mgr.GetAuto(id);
            List<Werk> toRemoveWerk = new List<Werk>();
            if(toRemove.AutoOnderdelen != null)
            {
                toRemove.AutoOnderdelen = new List<AutoOnderdeel>();
            }
            if(toRemove.WerkVoorAuto != null)
            {
                toRemoveWerk.AddRange(toRemove.WerkVoorAuto);
                toRemove.WerkVoorAuto = new List<Werk>();
            }
            mgr.ChangeAuto(toRemove);
            foreach(Werk werk in toRemoveWerk)
            {
                mgr.DeleteWerk(werk);
            }
            mgr.DeleteAuto(toRemove);

            
            return RedirectToAction("Index");
        }
        // GET
        public IActionResult Index()
        {
            IEnumerable<Auto> autos = mgr.GetAutos();
            return View(autos);
        }

        // GET: /Ticket/Details/<ticket_number>
        public IActionResult Details(int id)
        {
            Auto auto = mgr.GetAuto(id);
            List<Onderdeel> onderdelen = new List<Onderdeel>();
            foreach(AutoOnderdeel onderdeel in auto.AutoOnderdelen)
            {
                onderdelen.Add(mgr.GetOnderdeel(onderdeel.OnderdeelId));
            }
            AutoModel model = new AutoModel
            {
                OnderdelenVoorAuto = onderdelen,
                Auto = auto
            };
            return View(model);
        }

        public IActionResult Add()
        {
            return View(new Auto { Bouwjaar=2000});
        }

        [HttpPost]
        public IActionResult Add(Auto auto)
        {
            if (auto.Naam != null)
            {
                var added = mgr.AddAuto(auto);
                return RedirectToAction("Details", "Auto", new { id = added.AutoId });
            }
            return View();
        }
    }
}