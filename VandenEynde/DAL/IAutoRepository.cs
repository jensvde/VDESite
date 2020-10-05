using System.Collections;
using System.Collections.Generic;
using Domain;

namespace DAL
{
    public interface IAutoRepository
    {
        Auto CreateAuto(Auto auto);
        IEnumerable<Auto> ReadAutos();
        Auto ReadAuto(int autoId);

        void UpdateAuto(Auto auto);
        void DeleteAuto(int autoId);

        IEnumerable<Onderdeel> GetOnderdelenVanAuto(int autoId);
        Onderdeel CreateOnderdeel(Onderdeel onderdeel);
        void UpdateOnderdeel(Onderdeel onderdeel);
        void DeleteOnderdeel(Onderdeel onderdeel);

        IEnumerable<Werk> GetWerkVoorAuto(int autoId);
        Werk CreateWerk(Werk werk);

        IEnumerable<Onderdeel> ReadOnderdelen();
        IEnumerable<Werk> ReadWerken();

        Onderdeel ReadOnderdeel(int id);
        Werk ReadWerk(int id);
        void UpdateWerk(Werk werk);
        void DeleteWerk(Werk werk);

    }
}