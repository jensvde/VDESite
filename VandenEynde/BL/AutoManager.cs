using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using DAL;
using DAL.EF;
using Domain;

namespace BL
{
    public class AutoManager : IAutoManager
    {
        private readonly IAutoRepository repo;

        public AutoManager()
        {
            repo = new AutoRepository();
        }

        public IEnumerable<Auto> GetAutos()
        {
            return repo.ReadAutos();
        }

        public IEnumerable<Onderdeel> GetOnderdelen()
        {
            return repo.ReadOnderdelen();
        }

        public IEnumerable<Werk> GetWerken()
        {
            return repo.ReadWerken();
        }

        public void ChangeAuto(Auto auto)
        {
            repo.UpdateAuto(auto);
        }

        public Auto AddAuto(int AutoId, string Naam, int OlieInhoud, int Bouwjaar, int CylinderInhoud, int VermogenKw,
            string Motorcode, string Banden)
        {
            Auto auto = new Auto()
            {
                AutoId = AutoId,
                Naam = Naam,
                OlieInhoud = OlieInhoud,
                Bouwjaar = Bouwjaar,
                CylinderInhoud = CylinderInhoud,
                VermogenKw = VermogenKw,
                MotorCode = Motorcode,
                Banden = Banden
            };
            return this.AddAuto(auto);

        }

        public Auto GetAuto(int id)
        {
            return repo.ReadAuto(id);
        }

        public Auto AddAuto(int AutoId, string Naam)
        {
            Auto auto = new Auto()
            {
                AutoId = AutoId,
                Naam = Naam
            };
            return this.AddAuto(auto);
        }

        public Auto AddAuto(Auto auto)
        {
            return repo.CreateAuto(auto);
        }

        public void DeleteAuto(int id)
        {
            repo.DeleteAuto(id);
        }

        public Onderdeel AddOnderdeel(Onderdeel onderdeel)
        {
            return repo.CreateOnderdeel(onderdeel);
        }

        public void ChangeOnderdeel(Onderdeel onderdeel)
        {
            repo.UpdateOnderdeel(onderdeel);
        }

        public void DeleteOnderdeel(Onderdeel onderdeel)
        {
            repo.DeleteOnderdeel(onderdeel);
        }

        public Onderdeel GetOnderdeel(int id)
        {
            return repo.ReadOnderdeel(id);
        }

        public Werk GetWerk(int id)
        {
            return repo.ReadWerk(id);
        }

        public Werk AddWerk(Werk werk)
        {
            return repo.CreateWerk(werk);
        }

        public void ChangeWerk(Werk werk)
        {
            repo.UpdateWerk(werk);
        }

        public void DeleteWerk(Werk werk)
        {
            repo.DeleteWerk(werk);
        }
    }
}