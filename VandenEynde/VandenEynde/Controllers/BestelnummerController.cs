using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Microsoft.AspNetCore.Mvc;

namespace VandenEynde.Controllers
{
    public class BestelnummerController : Controller
    {
        private IAutoManager mgr = new AutoManager();
        public IActionResult Delete(int id)
        {
            var toDelete = mgr.GetOnderdeelBestelnummer(id);
            int idToReturn = toDelete.Onderdeel.OnderdeelId;
            mgr.DeleteOnderdeelBestelnummer(toDelete);
            return RedirectToAction("Details", "Onderdeel", new { id = idToReturn });
        }
    }
}
