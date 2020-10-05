using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;

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
            IEnumerable<Auto> autos = ctx.Autos.Include(e => e.Onderdelen).Include(e => e.WerkVoorAuto).AsEnumerable();
            return autos;
        }

        public IEnumerable<Onderdeel> ReadOnderdelen()
        {
            return ctx.Onderdelen.Include(e => e.Auto).AsEnumerable();
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
                .Include(auto => auto.Onderdelen)
                .Include(auto => auto.WerkVoorAuto)
                .Single(auto => auto.AutoId == autoId);
        }

        public void UpdateAuto(Auto auto)
        {
            ctx.Autos.Update(auto);
            ctx.SaveChanges();
        }

        public void DeleteAuto(int autoId)
        {
            Auto auto = ctx.Autos.Include(e => e.Onderdelen).Include(e => e.WerkVoorAuto)
                .Single(e => e.AutoId == autoId);
            ctx.Onderdelen.RemoveRange(auto.Onderdelen);
            ctx.Werken.RemoveRange(auto.WerkVoorAuto);
            ctx.Autos.Remove(auto);
            ctx.SaveChanges();
        }

        public IEnumerable<Onderdeel> GetOnderdelenVanAuto(int autoId)
        {
            IEnumerable<Onderdeel> foundOnderdelen = ctx.Onderdelen
                .Where(x => x.Auto.AutoId == autoId)
                .ToList();
            return foundOnderdelen;
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

        public IEnumerable<Werk> GetWerkVoorAuto(int autoId)
        {
            IEnumerable<Werk> foundWerk = ctx.Werken.Where(x => x.Auto.AutoId == autoId);
            return foundWerk;
        }

        public Werk CreateWerk(Werk werk)
        {
            ctx.Werken.Add(werk);
            ctx.SaveChanges();
            return werk;
        }

        public Onderdeel ReadOnderdeel(int id)
        {
            return ctx.Onderdelen.Single(x => x.OnderdeelId == id);
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
    }
}