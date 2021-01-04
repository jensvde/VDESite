using Domain;
using System.Collections.Generic;

namespace DAL
{
    public interface IAutoRepository
    {
        Auto CreateAuto(Auto auto);
        IEnumerable<Auto> ReadAutos();
        Auto ReadAuto(int autoId);

        void UpdateAuto(Auto auto);
        void DeleteAuto(Auto auto);

        Onderdeel CreateOnderdeel(Onderdeel onderdeel);
        void UpdateOnderdeel(Onderdeel onderdeel);
        void DeleteOnderdeel(Onderdeel onderdeel);

        Werk CreateWerk(Werk werk);

        IEnumerable<Onderdeel> ReadOnderdelen();
        IEnumerable<Werk> ReadWerken();

        Onderdeel ReadOnderdeel(int id);
        Werk ReadWerk(int id);
        void UpdateWerk(Werk werk);
        void DeleteWerk(Werk werk);

        IEnumerable<OnderdeelBestelnummer> ReadonderdeelBestelnummers();
        OnderdeelBestelnummer ReadOnderdeelBestelnummer(int id);
        void UpdateOnderdeelBestelnummer(OnderdeelBestelnummer onderdeelBestelnummer);
        void DeleteOnderdeelBestelnummer(OnderdeelBestelnummer onderdeelBestelnummer);
        OnderdeelBestelnummer CreateOnderdeelBestelnummer(OnderdeelBestelnummer onderdeelBestelnummer);

    }
}