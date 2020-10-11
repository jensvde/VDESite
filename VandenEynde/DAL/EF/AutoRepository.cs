using Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.EF
{
    public class AutoRepository : IAutoRepository
    {
        private VandenEyndeDbContext ctx = null;

        public AutoRepository()
        {
            ctx = new VandenEyndeDbContext();
            VandenEyndeDbContext.Initialize(ctx, dropCreateDatabase: true);
        }
        public Auto CreateAuto(Auto auto)
        {
            ctx.Autos.Add(auto);
            ctx.SaveChanges();
            return auto;
        }

        public IEnumerable<Auto> ReadAutos()
        {
            IEnumerable<Auto> autos = ctx.Autos.Include(u => u.AutoOnderdelen).Include(u => u.WerkVoorAuto).AsEnumerable();
            return autos;
        }

        public IEnumerable<Onderdeel> ReadOnderdelen()
        {
            return ctx.Onderdelen.Include(e => e.Bestelnummers).Include(e => e.AutoOnderdelen).AsEnumerable();
        }

        public IEnumerable<Werk> ReadWerken()
        {
            return ctx.Werken.Include(e => e.Auto).AsEnumerable();
        }
        public Auto ReadAuto(int autoId)
        {
            /*
            Auto auto = ctx.Autos.Single(x => x.AutoId == autoId);
            auto.Onderdelen = GetOnderdelenVanAuto(autoId).ToList();
            auto.WerkVoorAuto = GetWerkVoorAuto(autoId).ToList();
            return auto;*/

            return ctx.Autos
                .Include(auto => auto.AutoOnderdelen)
                .Include(auto => auto.WerkVoorAuto)
                .Single(auto => auto.AutoId == autoId);
        }

        public void UpdateAuto(Auto auto)
        {
            ctx.Autos.Update(auto);
            ctx.SaveChanges();
        }

        public void DeleteAuto(Auto auto)
        {

            ctx.Autos.Remove(auto);
            ctx.SaveChanges();
        }


        public Onderdeel CreateOnderdeel(Onderdeel onderdeel)
        {
            ctx.Onderdelen.Add(onderdeel);
            ctx.SaveChanges();
            return onderdeel;
        }

        public void UpdateOnderdeel(Onderdeel onderdeel)
        {
            ctx.Onderdelen.Update(onderdeel);
            ctx.SaveChanges();
        }

        public void DeleteOnderdeel(Onderdeel onderdeel)
        {
            ctx.Onderdelen.Remove(onderdeel);
            ctx.SaveChanges();
        }


        public Werk CreateWerk(Werk werk)
        {
            ctx.Werken.Add(werk);
            ctx.SaveChanges();
            return werk;
        }

        public Onderdeel ReadOnderdeel(int id)
        {
            return ctx.Onderdelen.Include(e => e.Bestelnummers).Include(e => e.AutoOnderdelen).Single(x => x.OnderdeelId == id);
        }

        public Werk ReadWerk(int id)
        {
            return ctx.Werken.Single(x => x.WerkId == id);
        }

        public void UpdateWerk(Werk werk)
        {
            ctx.Werken.Update(werk);
            ctx.SaveChanges();
        }

        public void DeleteWerk(Werk werk)
        {
            ctx.Werken.Remove(werk);
            ctx.SaveChanges();
        }
        public IEnumerable<OnderdeelBestelnummer> ReadonderdeelBestelnummers()
        {
            return ctx.Bestelnummers.Include(e => e.Onderdeel).AsEnumerable();
        }
        public OnderdeelBestelnummer ReadOnderdeelBestelnummer(int id)
        {
            return ctx.Bestelnummers.Include(e => e.Onderdeel).Single(x => x.BestelnummerId == id);
        }

        public void UpdateOnderdeelBestelnummer(OnderdeelBestelnummer onderdeelBestelnummer)
        {
            ctx.Bestelnummers.Update(onderdeelBestelnummer);
            ctx.SaveChanges();
        }

        public void DeleteOnderdeelBestelnummer(OnderdeelBestelnummer onderdeelBestelnummer)
        {
            ctx.Bestelnummers.Remove(onderdeelBestelnummer);
            ctx.SaveChanges();
        }

        public OnderdeelBestelnummer CreateOnderdeelBestelnummer(OnderdeelBestelnummer onderdeelBestelnummer)
        {
            ctx.Bestelnummers.Add(onderdeelBestelnummer);
            ctx.SaveChanges();
            return onderdeelBestelnummer;
        }
    }
}