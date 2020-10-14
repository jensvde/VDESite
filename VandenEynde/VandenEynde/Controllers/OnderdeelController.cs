using BL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using VandenEynde.Models;

namespace VandenEynde.Controllers
{
    // [Authorize]
    public class OnderdeelController : Controller
    {
        private IAutoManager mgr;
        private OnderdeelIndexModel createIndexModel(int id = 0)
        {

            OnderdeelIndexModel model = new OnderdeelIndexModel();
                var OnderdelenQuery = mgr.GetOnderdelen();
                List<Onderdeel> Onderdelen = new List<Onderdeel>();
                foreach (Onderdeel onderdeel in OnderdelenQuery.OrderBy(x => x.Beschrijving).OrderBy(x => x.Merk))
                {
                    if (id == 0)
                    {
                        Onderdelen.Add(onderdeel);
                    }
                    else
                    {
                        foreach(AutoOnderdeel autoOnderdeel in onderdeel.AutoOnderdelen)
                        {
                            if (autoOnderdeel.AutoId == id)
                            {
                                Onderdelen.Add(onderdeel);
                            }
                        }
                    }
                }
                int count = 0;
                foreach (Onderdeel onderdeel in Onderdelen)
                {
                    model.Onderdelen.Add(onderdeel);
                    foreach (AutoOnderdeel autoOnderdeel in onderdeel.AutoOnderdelen)
                    {
                        model.Autos[count] += mgr.GetAuto(autoOnderdeel.AutoId).Naam + "\n";
                    }
                    var bestelnrs = onderdeel.Bestelnummers;
                    foreach (OnderdeelBestelnummer bestelnummer in bestelnrs)
                    {
                        model.Bestelnummers[count] += bestelnummer.Nr + "\n";
                    }
                    count++;
                }
            string[] checker = new string[model.Onderdelen.Count+1];
            count = 0;
                foreach(Onderdeel onderdeel in model.Onderdelen)
            {
                if (!checker.Contains(onderdeel.Merk))
                {
                    model.GelesecteerdeMerken[count] =
                        new OnderdeelModel
                        {
                            Name = onderdeel.Merk
                        } ;
                    checker[count] = onderdeel.Merk;
                    count++;
                }
            }
            return model;
        }
        public OnderdeelController()
        {
            mgr = new AutoManager();
        }
        // GET
        public IActionResult Index()
        {
            return View(createIndexModel());
        }

        [HttpPost]
        public IActionResult Index(int id)
        {
            return View(createIndexModel(id));
        }

        private OnderdeelViewModel createDetailsModel(int id)
        {
            OnderdeelViewModel onderdeelViewModel = new OnderdeelViewModel
            {
                Onderdeel = mgr.GetOnderdeel(id),
                AvailableAutos = new List<SelectListItem>()
            };

            List<int> excludedAutos = new List<int>();
            foreach(AutoOnderdeel autoOnderdeel in onderdeelViewModel.Onderdeel.AutoOnderdelen)
            {
                excludedAutos.Add(autoOnderdeel.AutoId);
            }
            foreach(Auto auto in mgr.GetAutos())
            {
                if (!(excludedAutos.Contains(auto.AutoId)))
                {
                    onderdeelViewModel.AvailableAutos.Add(new SelectListItem
                    {
                        Text = auto.Naam, 
                        Value = ""+auto.AutoId
                    });  
                }
                
            }

            int count = 0;
            foreach(OnderdeelBestelnummer bestelnummer in mgr.GetOnderdeel(id).Bestelnummers)
            {
                onderdeelViewModel.OnderdeelBestelnummers[count] = bestelnummer;
                count++;
            }
            return onderdeelViewModel;
        }
        public IActionResult Details(int id)
        {
            return View(createDetailsModel(id));
        }


        

        [HttpPost]
        public IActionResult Edit(OnderdeelViewModel model)
        {
                Onderdeel onderdeel = mgr.GetOnderdeel(model.Onderdeel.OnderdeelId);
                if (onderdeel != null)
                {
                    //////////////////ONDERDEEL/////////////////////////
                    onderdeel.Merk = model.Onderdeel.Merk;
                    onderdeel.Beschrijving = model.Onderdeel.Beschrijving;
                    if (model.OnderdeelBestelnummers != null)
                    {

                        int count = 0;
                        foreach (OnderdeelBestelnummer bestelnr in onderdeel.Bestelnummers)
                        {
                            bestelnr.Nr = model.OnderdeelBestelnummers[count].Nr;
                            mgr.ChangeOnderdeelBestelnummer(bestelnr);
                            count++;
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////AUTOS//////////////////////////////////////
                    if (model.SelectedAutos != null)
                    {
                        foreach (int idAuto in model.SelectedAutos)
                        {
                            onderdeel.AutoOnderdelen.Add(new AutoOnderdeel
                            {
                                Auto = mgr.GetAuto(idAuto),
                                Onderdeel = onderdeel
                            });
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////BESTELNUMMERS///////////////////////////////
                    if (model.NieuweBestelnummers != null)
                    {
                        if (onderdeel.Bestelnummers == null)
                        {
                            onderdeel.Bestelnummers = new List<OnderdeelBestelnummer>();
                        }
                        foreach (string nummer in model.NieuweBestelnummers)
                        {
                            if (nummer != null)
                            {
                                OnderdeelBestelnummer onderdeelBestelnummer = new OnderdeelBestelnummer
                                {
                                    Onderdeel = onderdeel,
                                    Nr = nummer
                                };
                                mgr.AddOnderdeelBestelnummer(onderdeelBestelnummer);
                            }
                        }
                    }

                    mgr.ChangeOnderdeel(onderdeel);
                }
            
            return RedirectToAction("Details", "Onderdeel", new { id = onderdeel.OnderdeelId });

        }

        public IActionResult Delete(int id)
        {

            mgr.DeleteOnderdeel(mgr.GetOnderdeel(id));
            return RedirectToAction("Index", "Onderdeel");
        }

        public IActionResult DeleteAuto(int id, int auto)
        {
            Onderdeel onderdeelToUpdate = mgr.GetOnderdeel(id);
            List<AutoOnderdeel> toRemove = new List<AutoOnderdeel>();
            foreach(AutoOnderdeel autoOnderdeel in onderdeelToUpdate.AutoOnderdelen)
            {
                if (autoOnderdeel.AutoId == auto)
                {
                    toRemove.Add(autoOnderdeel);
                }
            }
            foreach(AutoOnderdeel removeThis in toRemove)
            {
                onderdeelToUpdate.AutoOnderdelen.Remove(removeThis);
            }
            mgr.ChangeOnderdeel(onderdeelToUpdate);
            return RedirectToAction("Details", "Onderdeel", new { id = onderdeelToUpdate.OnderdeelId });
        }

        public IActionResult Add(int id)
        {
            OnderdeelViewModel onderdeelViewModel = new OnderdeelViewModel
            {
                AvailableAutos = new List<SelectListItem>(),
                Onderdeel = new Onderdeel()
            };
            onderdeelViewModel.Onderdeel.AutoOnderdelen = new List<AutoOnderdeel>();

            List<int> excludedAutos = new List<int>();
            foreach (AutoOnderdeel autoOnderdeel in onderdeelViewModel.Onderdeel.AutoOnderdelen)
            {
                excludedAutos.Add(autoOnderdeel.AutoId);
            }
            if (excludedAutos.Count == 0)
            {
                if (id != 0)
                {
                    excludedAutos.Add(id);
                }
            }
            foreach (Auto auto in mgr.GetAutos())
            {
                if (!(excludedAutos.Contains(auto.AutoId)))
                {
                    onderdeelViewModel.AvailableAutos.Add(new SelectListItem
                    {
                        Text = auto.Naam,
                        Value = "" + auto.AutoId
                    });
                }

            }
            onderdeelViewModel.Onderdeel.Bestelnummers = new List<OnderdeelBestelnummer>
            {
                new OnderdeelBestelnummer()
            };
            if(id > 0)
            {
                onderdeelViewModel.Auto = mgr.GetAuto(id);
            }
            return View(onderdeelViewModel);
        }

        [HttpPost]
        public IActionResult Add(OnderdeelViewModel model)
        {

            Onderdeel toAdd = model.Onderdeel;
            ///////////////////////////////////////////////////////////////////////////
            ///////////////////////////////AUTOS//////////////////////////////////////
            if (toAdd.AutoOnderdelen == null) { toAdd.AutoOnderdelen = new List<AutoOnderdeel>(); }
            if (model.SelectedAutos != null)
            {
                if(model.Auto != null && model.Auto.AutoId > 0) {
                    toAdd.AutoOnderdelen.Add(new AutoOnderdeel
                    {
                        Auto = mgr.GetAuto(model.Auto.AutoId),
                        Onderdeel = toAdd
                    });
                }
                foreach (int idAuto in model.SelectedAutos)
                {
                    toAdd.AutoOnderdelen.Add(new AutoOnderdeel
                    {
                        Auto = mgr.GetAuto(idAuto),
                        Onderdeel = toAdd
                    });
                }
            }
            toAdd = mgr.AddOnderdeel(toAdd);

            toAdd.Bestelnummers = new List<OnderdeelBestelnummer>();
            ///////////////////////////////////////////////////////////////////////////
            ///////////////////////////////BESTELNUMMERS///////////////////////////////
            if (model.Bestelnummer != null)
            {
                toAdd.Bestelnummers.Add(model.Bestelnummer);
            }

            if (model.NieuweBestelnummers != null)
            {
                
                foreach (string nummer in model.NieuweBestelnummers)
                {
                    if (nummer != null && nummer.Length > 1)
                    {
                        OnderdeelBestelnummer onderdeelBestelnummer = new OnderdeelBestelnummer
                        {
                            Onderdeel = toAdd,
                            Nr = nummer
                        };
                        mgr.AddOnderdeelBestelnummer(onderdeelBestelnummer);
                    }
                }
            }
            mgr.ChangeOnderdeel(toAdd);
            
            return RedirectToAction("Details", "Onderdeel", new { id = toAdd.OnderdeelId });

        }

    }
}