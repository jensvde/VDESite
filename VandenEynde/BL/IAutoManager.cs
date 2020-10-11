using Domain;
using System.Collections.Generic;

namespace BL
{
    public interface IAutoManager
    {
        IEnumerable<Auto> GetAutos();
        Auto AddAuto(int AutoId, string Naam);
        Auto AddAuto(int AutoId, string Naam, int OlieInhoud, int Bouwjaar, int CylinderInhoud, int VermogenKw, string Motorcode, string Banden);
        Auto AddAuto(Auto auto);
        Auto GetAuto(int id);

        IEnumerable<Onderdeel> GetOnderdelen();
        IEnumerable<Werk> GetWerken();

        void ChangeAuto(Auto auto);
        void DeleteAuto(Auto auto);

        Onderdeel AddOnderdeel(Onderdeel onderdeel);
        void ChangeOnderdeel(Onderdeel onderdeel);
        void DeleteOnderdeel(Onderdeel onderdeel);

        Onderdeel GetOnderdeel(int id);
        Werk GetWerk(int id);
        Werk AddWerk(Werk werk);
        void ChangeWerk(Werk werk);
        void DeleteWerk(Werk werk);

        OnderdeelBestelnummer GetOnderdeelBestelnummer(int id);
        OnderdeelBestelnummer AddOnderdeelBestelnummer(OnderdeelBestelnummer onderdeelBestelnummer);
        void ChangeOnderdeelBestelnummer(OnderdeelBestelnummer onderdeelBestelnummer);
        void DeleteOnderdeelBestelnummer(OnderdeelBestelnummer onderdeelBestelnummer);
        IEnumerable<OnderdeelBestelnummer> GetOnderdeelBestelnummers();
    }
}