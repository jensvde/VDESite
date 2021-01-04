using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace VandenEynde.Models
{
    public class OnderdeelViewModel
    {
        public string[] NieuweBestelnummers { get; set; }
        public OnderdeelBestelnummer[] OnderdeelBestelnummers { get; set; }
        public OnderdeelBestelnummer Bestelnummer { get; set; }
        public int[] SelectedAutos { get; set; }
        public Auto Auto 
        {
            get; set;
        }
        public Onderdeel Onderdeel { get; set; }
        public IList<SelectListItem> AvailableAutos { get; set; }

        public OnderdeelViewModel()
        {
            this.NieuweBestelnummers = new string[10];
            this.SelectedAutos = new int[10];
            this.OnderdeelBestelnummers = new OnderdeelBestelnummer[10];
            this.Bestelnummer = new OnderdeelBestelnummer();
        }
    }
}
