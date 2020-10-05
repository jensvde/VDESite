using System.Collections;
using System.Collections.Generic;
using BL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            mgr.DeleteAuto(id);
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
            return View(auto);
        }

        public IActionResult EditOnderdeel(int id)
        {
            return RedirectToAction("New", "Onderdeel", new {id = id});
        }

        public IActionResult NewOnderdeel(Auto auto)
        {
            int id = auto.AutoId;
            return RedirectToAction("New", "Onderdeel", new {id = id});
        }

        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Add(Auto auto)
        {
            Auto addedAuto = mgr.AddAuto(auto);
            return RedirectToAction("Details", "Auto", new {id = auto.AutoId});
        }
    }
}