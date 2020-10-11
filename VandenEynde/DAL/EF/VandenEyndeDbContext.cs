using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.EF
{
    internal class VandenEyndeDbContext : DbContext
    {
        private static bool hasRunDuringAppExecution = false;

        public DbSet<Auto> Autos { get; set; }
        public DbSet<Onderdeel> Onderdelen { get; set; }
        public DbSet<Werk> Werken { get; set; }
        public DbSet<OnderdeelBestelnummer> Bestelnummers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=VandenEyndeDb_EFCodeFirst.db");
            optionsBuilder.UseMySql("server=localhost;database=db_vde;user=winkel;password=Winkeltje@1234");



        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Werk>(entity =>
            {
                entity.HasOne(e => e.Auto).WithMany(e => e.WerkVoorAuto);
            });

            modelBuilder.Entity<Onderdeel>(entity =>
            {
             //   entity.HasMany(e => e.AutoOnderdelen).WithOne(e => e.Onderdeel);
            });



            modelBuilder.Entity<Auto>(entity =>
            {
                entity.HasKey(e => e.AutoId);
                entity.Property(e => e.AutoId).IsRequired();
                entity.HasMany<Werk>(e => e.WerkVoorAuto).WithOne(e => e.Auto);
               // entity.HasMany<AutoOnderdeel>(e => e.AutoOnderdelen).WithOne(e => e.Auto);
            });

            modelBuilder.Entity<AutoOnderdeel>()
               .HasKey(t => new { t.AutoId, t.OnderdeelId });

            modelBuilder.Entity<AutoOnderdeel>()
                .HasOne(pt => pt.Auto)
                .WithMany(p => p.AutoOnderdelen)
                .HasForeignKey(pt => pt.AutoId);

            modelBuilder.Entity<AutoOnderdeel>()
                .HasOne(pt => pt.Onderdeel)
                .WithMany(t => t.AutoOnderdelen)
                .HasForeignKey(pt => pt.OnderdeelId);
        }
        public static void Initialize(VandenEyndeDbContext context, bool dropCreateDatabase = false)
        {
            if (!hasRunDuringAppExecution)
            {
                // Delete database if requested
                if (dropCreateDatabase)
                    context.Database.EnsureDeleted();
                // Create database and initial data if needed
                if (context.Database.EnsureCreated())
                    Seed(context);
                hasRunDuringAppExecution = true;
            }
        }
        private static void Seed(VandenEyndeDbContext context)
        {
            Auto auto1 = new Auto
            {
                Naam = "Golf Plus JP",
                OlieInhoud = 4.5,
                Bouwjaar = 2005,
                CylinderInhoud = 1900,
                VermogenKw = 77,
                MotorCode = "BKC",
                Banden = "195/65/15  91H",
                DatumAangemaakt = DateTime.Today
            };
            Auto auto2 = new Auto
            {
                Naam = "H1 Eddy",
                OlieInhoud = 7.5,
                Bouwjaar = 2011,
                CylinderInhoud = 2500,
                VermogenKw = 120,
                MotorCode = "",
                Banden = "215/70/R16C  108/106T",
                DatumAangemaakt = DateTime.Today
            };
           
            context.Autos.AddRange(new[] { auto1, auto2 });
            context.SaveChanges();
            OnderdeelBestelnummer bestel1 = new OnderdeelBestelnummer
            {
                Nr = "P9192         1457429192"
            };
            OnderdeelBestelnummer bestel2 = new OnderdeelBestelnummer
            {
                Nr = "P9dfdddddd9192"
            };
            OnderdeelBestelnummer bestel3 = new OnderdeelBestelnummer
            {
                Nr = "P9dfgdfgdfg192"
            };
            OnderdeelBestelnummer bestel4 = new OnderdeelBestelnummer
            {
                Nr = "Pdfgdfg"
            };
            context.Bestelnummers.AddRange(new[] { bestel1, bestel2, bestel3, bestel4 });
            context.SaveChanges();

            Onderdeel onderdeel_auto1_1 = new Onderdeel
            {
                Beschrijving = "Olie filter",
                Merk = "Bosch",
                Bestelnummers = new List<OnderdeelBestelnummer>(),
                AutoOnderdelen = new List<AutoOnderdeel>()
            };
            onderdeel_auto1_1.Bestelnummers.Add(bestel1);
            onderdeel_auto1_1.Bestelnummers.Add(bestel3);
            Onderdeel onderdeel_auto1_2 = new Onderdeel
            {
                Beschrijving = "Lucht filter",
                Merk = "Bosch",
                Bestelnummers = new List<OnderdeelBestelnummer>(),
                AutoOnderdelen = new List<AutoOnderdeel>()
            };
            onderdeel_auto1_2.Bestelnummers.Add(bestel2);
            onderdeel_auto1_2.Bestelnummers.Add(bestel4);

            context.Onderdelen.AddRange(new[] { onderdeel_auto1_1, onderdeel_auto1_2 });
            context.SaveChanges();
            Werk werk1 = new Werk
            {
                Auto = auto2,
                Datum = new DateTime(2020, 6, 11),
                KilometerStand = 175000,
                OliefilterVervangen = true,
                Extra = "Remblokken achter"
            };
            Werk werk2 = new Werk
            {
                Auto = auto2,
                Datum = new DateTime(2020, 06, 11),
                OliefilterVervangen = true,
                KilometerStand = 185000
            };
            context.Werken.AddRange(new[] { werk1, werk2 });
            context.SaveChanges();
            foreach (Auto auto in context.Autos.ToList())
            {
                auto.AutoOnderdelen = new List<AutoOnderdeel>(new[] { new AutoOnderdeel
                {
                    Auto = auto,
                    Onderdeel = onderdeel_auto1_1
                }, new AutoOnderdeel
                {
                    Auto = auto,
                    Onderdeel = onderdeel_auto1_2
                } });
                context.Autos.Update(auto);
            }

            context.SaveChanges();

            Auto auto3 = new Auto
            {
                Naam = "Ford Ranger",
                OlieInhoud = 2.5,
                Bouwjaar = 2016,
                CylinderInhoud = 2895,
                VermogenKw = 100,
                MotorCode = "",
                Banden = "",
                DatumAangemaakt = DateTime.Today
            };
            context.Autos.Add(auto3);
            context.SaveChanges();

            foreach (EntityEntry entry in context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }
        /*
        private static void Seed(VandenEyndeDbContext context)
        {
            //Autos
            Auto auto1 = new Auto
            {
                Naam = "Golf Plus JP", OlieInhoud = 4.5, Bouwjaar = 2005, CylinderInhoud = 1900, VermogenKw = 77, MotorCode = "BKC", Banden = "195/65/15  91H", DatumAangemaakt = DateTime.Today
            };
            Auto auto2 = new Auto
            {
                Naam = "H1 Eddy", OlieInhoud = 7.5, Bouwjaar = 2011, CylinderInhoud = 2500, VermogenKw = 120, MotorCode = "", Banden = "215/70/R16C  108/106T", DatumAangemaakt = DateTime.Today
            };
            Auto auto3 = new Auto
            {
                Naam = "Ford Ranger", OlieInhoud = 2.5, Bouwjaar = 2016, CylinderInhoud = 2895, VermogenKw = 100, MotorCode = "", Banden = "", DatumAangemaakt = DateTime.Today
            };
            Auto auto4 = new Auto
            {
                Naam = "207 Nadine", Bouwjaar = 2010, CylinderInhoud = 14, VermogenKw = 50, MotorCode = "C8HZ", Banden = "185/65R15/88H", DatumAangemaakt = DateTime.Today
            };
            Auto auto5 = new Auto
            {
                Naam = "Captiva Eric", Bouwjaar = 2008, CylinderInhoud = 2000, VermogenKw = 93, MotorCode = "", Banden = "", DatumAangemaakt = DateTime.Today
            };
            Auto auto6 = new Auto
            {
                Naam = "C220 Sabrina", OlieInhoud = 6.5, Bouwjaar = 2008, CylinderInhoud = 2148, VermogenKw = 120, MotorCode = "", Banden = "", DatumAangemaakt = DateTime.Today
            };
            Auto auto7 = new Auto
            {
                Naam = "C Max Voet", Bouwjaar = 2014, CylinderInhoud = 1600, VermogenKw = 85, MotorCode = "T1DB1H", Banden = "", DatumAangemaakt = DateTime.Today
            };
            Auto auto8 = new Auto
            {
                Naam = "Tucson Patje", OlieInhoud = 5.5, Bouwjaar = 2016, CylinderInhoud = 1700, VermogenKw = 85, MotorCode = "D4FD", Banden = "", DatumAangemaakt = DateTime.Today
            };
            Auto auto9 = new Auto
            {
                Naam = "berlingo wit 01 kris", OlieInhoud = 5, Bouwjaar = 2001, CylinderInhoud = 1900, MotorCode = "MBWJYB", Banden = "", DatumAangemaakt = DateTime.Today
            };
            Auto auto10 = new Auto
            {
                Naam = "Octavia Kris", OlieInhoud = 4.5,Bouwjaar = 2011, CylinderInhoud = 1600, VermogenKw = 77, MotorCode = "CAY", Banden = "", DatumAangemaakt = DateTime.Today
            };
            Auto auto11 = new Auto
            {
                Naam = "Golf Ilse", OlieInhoud = 4.5,Bouwjaar = 2007, CylinderInhoud = 2000, MotorCode = "", Banden = "", DatumAangemaakt = DateTime.Today
            };
            Auto auto12 = new Auto
            {
                Naam = "polo zwart jansens", OlieInhoud = 4.3, Bouwjaar = 2010, CylinderInhoud = 1600, VermogenKw = 55, MotorCode = "CAY", Banden = "", DatumAangemaakt = DateTime.Today
            };
            Auto auto13 = new Auto
            {
                Naam = "fabia Ron", OlieInhoud = 4.4,Bouwjaar = 2008, CylinderInhoud = 1400, VermogenKw = 51, MotorCode = "", Banden = "", DatumAangemaakt = DateTime.Today
            };
            Auto auto14 = new Auto
            {
                Naam = "fiesta magda", OlieInhoud = 4.2,Bouwjaar = 2010, MotorCode = "", Banden = "", DatumAangemaakt = DateTime.Today
            };
            Auto auto15 = new Auto
            {
                Naam = "208 Jens", OlieInhoud = 4.25, Bouwjaar = 2012, CylinderInhoud = 1600, VermogenKw = 115, MotorCode = "CA5FV8", Banden = "195/55/16  91V", DatumAangemaakt = DateTime.Today
            };


            //Onderdelen
            //Auto1
            Onderdeel onderdeel_auto1_1 = new Onderdeel
            {
                Auto = auto1, Beschrijving = "Olie filter", Merk = "Bosch", BestelNr = "P9192         1457429192"
            };
            Onderdeel onderdeel_auto1_2 = new Onderdeel
            {
                Auto = auto1, Beschrijving = "Lucht filter", Merk = "Bosch", BestelNr = "1987429404"
            };
            Onderdeel onderdeel_auto1_3 = new Onderdeel
            {
                Auto = auto1, Beschrijving = "Brandstof filter", Merk = "Bosch", BestelNr = "1457070007"
            };
            Onderdeel onderdeel_auto1_4 = new Onderdeel
            {
                Auto = auto1, Beschrijving = "Interieur filter", Merk = "Bosch", BestelNr = "1987432097"
            };
            List<Onderdeel> onderdelen_auto1 = new List<Onderdeel>
            {
                onderdeel_auto1_1,onderdeel_auto1_2,onderdeel_auto1_3,onderdeel_auto1_4
            };
            //Auto2
            Onderdeel onderdeel_auto2_1 = new Onderdeel
            {
                Auto = auto2, Beschrijving = "Olie filter", Merk = "AMC", BestelNr = "KO-094 / HO-607 /MO-400"
            };
            Onderdeel onderdeel_auto2_2 = new Onderdeel
            {
                Auto = auto2, Beschrijving = "Lucht filter", Merk = "AMC", BestelNr = "HA-720"
            };
            Onderdeel onderdeel_auto2_3 = new Onderdeel
            {
                Auto = auto2, Beschrijving = "Brandstof filter", Merk = "AMC", BestelNr = "KF1478"
            };
            Onderdeel onderdeel_auto2_4 = new Onderdeel
            {
                Auto = auto2, Beschrijving = "Interieur filter", Merk = "AMC", BestelNr = "HC 80219"
            };
            Onderdeel onderdeel_auto2_5 = new Onderdeel
            {
                Auto = auto2, Beschrijving = "Remblokken achter", Merk = "Coldax", BestelNr = "CB 210487"
            };
            List<Onderdeel> onderdelen_auto2 = new List<Onderdeel>
            {
                onderdeel_auto2_1,onderdeel_auto2_2,onderdeel_auto2_3,onderdeel_auto2_4,onderdeel_auto2_5
            };
            //Auto5
            Onderdeel onderdeel_auto5_1 = new Onderdeel
            {
                Auto = auto5, Beschrijving = "Olie filter", Merk = "Bosch", BestelNr = "P 7001"
            };
            Onderdeel onderdeel_auto5_2 = new Onderdeel
            {
                Auto = auto5, Beschrijving = "Lucht filter", Merk = "Bosch", BestelNr = "S 0214"
            };
            Onderdeel onderdeel_auto5_3 = new Onderdeel
            {
                Auto = auto5, Beschrijving = "Brandstof filter", Merk = "Bosch", BestelNr = "N 4437"
            };
            Onderdeel onderdeel_auto5_4 = new Onderdeel
            {
                Auto = auto5, Beschrijving = "Interieur filter", Merk = "Bosch", BestelNr = "A 8500"
            };
            List<Onderdeel> onderdelen_auto5 = new List<Onderdeel>
            {
                onderdeel_auto5_1,onderdeel_auto5_2,onderdeel_auto5_3,onderdeel_auto5_4
            };
            //Auto7
            Onderdeel onderdeel_auto7_1 = new Onderdeel
            {
                Auto = auto7, Beschrijving = "Lucht filter", Merk = "Bosch", BestelNr = "F 026 400 492"
            };
            Onderdeel onderdeel_auto7_2 = new Onderdeel
            {
                Auto = auto7, Beschrijving = "Interieur filter", Merk = "Bosch", BestelNr = "1 987 435 018"
            };
            List<Onderdeel> onderdelen_auto7 = new List<Onderdeel>
            {
                onderdeel_auto7_1,onderdeel_auto7_2
            };
            //Auto8
            Onderdeel onderdeel_auto8_1 = new Onderdeel
            {
                Auto = auto8, Beschrijving = "Olie filter", Merk = "Bosch", BestelNr = "F 026 407 147"
            };
            Onderdeel onderdeel_auto8_2 = new Onderdeel
            {
                Auto = auto8, Beschrijving = "Lucht filter", Merk = "AMC", BestelNr = "HA 743"
            };
            Onderdeel onderdeel_auto8_3 = new Onderdeel
            {
                Auto = auto8, Beschrijving = "Brandstof filter", Merk = "AMC", BestelNr = "HF 616"
            };
            Onderdeel onderdeel_auto8_4 = new Onderdeel
            {
                Auto = auto8, Beschrijving = "Remblokken voor", Merk = "Bosch", BestelNr = "0986 494 559"
            };
            Onderdeel onderdeel_auto8_5 = new Onderdeel
            {
                Auto = auto8, Beschrijving = "Pollen filter", Merk = "AMC", BestelNr = "HC 8240"
            };
            List<Onderdeel> onderdelen_auto8 = new List<Onderdeel>
            {
                onderdeel_auto8_1,onderdeel_auto8_2,onderdeel_auto8_3,onderdeel_auto8_4,onderdeel_auto8_5
            };
            //Auto9
            Onderdeel onderdeel_auto9_1 = new Onderdeel
            {
                Auto = auto9, Beschrijving = "Olie filter", Merk = "Knecht", BestelNr = "OC 100  OC 976"
            };
            List<Onderdeel> onderdelen_auto9 = new List<Onderdeel>
            {
                onderdeel_auto9_1
            };
            //Auto10
            Onderdeel onderdeel_auto10_1 = new Onderdeel
            {
                Auto = auto10, Beschrijving = "Olie filter", Merk = "Bosch", BestelNr = "F026 407 023    OX388D"
            };
            Onderdeel onderdeel_auto10_2 = new Onderdeel
            {
                Auto = auto10, Beschrijving = "Lucht filter", Merk = "Bosch", BestelNr = "1987 429 404"
            };
            Onderdeel onderdeel_auto10_3 = new Onderdeel
            {
                Auto = auto10, Beschrijving = "Brandstof filter", Merk = "Bosch", BestelNr = "1457 070 008         KX220D"
            };
            Onderdeel onderdeel_auto10_4 = new Onderdeel
            {
                Auto = auto10, Beschrijving = "Interieur filter", Merk = "Bosch", BestelNr = "1987 432 097"
            };
            Onderdeel onderdeel_auto10_5 = new Onderdeel
            {
                Auto = auto10, Beschrijving = "Remblokken voor", Merk = "Bosch", BestelNr = "0986 494 019"
            };
            List<Onderdeel> onderdelen_auto10 = new List<Onderdeel>
            {
                onderdeel_auto10_1,onderdeel_auto10_2,onderdeel_auto10_3,onderdeel_auto10_4,onderdeel_auto10_5
            };
            //Auto11
            Onderdeel onderdeel_auto11_1 = new Onderdeel
            {
                Auto = auto11, Beschrijving = "Olie filter", Merk = "Bosch", BestelNr = "1 457 429 192"
            };
            List<Onderdeel> onderdelen_auto11 = new List<Onderdeel>
            {
                onderdeel_auto11_1
            };
            //Auto12
            Onderdeel onderdeel_auto12_1 = new Onderdeel
            {
                Auto = auto12, Beschrijving = "Olie filter", Merk = "Bosch", BestelNr = "F 026 407 023"
            };
            Onderdeel onderdeel_auto12_2 = new Onderdeel
            {
                Auto = auto12, Beschrijving = "Lucht filter", Merk = "Bosch", BestelNr = "F 026 400 391"
            };
            Onderdeel onderdeel_auto12_3 = new Onderdeel
            {
                Auto = auto12, Beschrijving = "Brandstof filter", Merk = "Bosch", BestelNr = "0 450 906 500"
            };
            Onderdeel onderdeel_auto12_4 = new Onderdeel
            {
                Auto = auto12, Beschrijving = "Interieur filter", Merk = "Bosch", BestelNr = "1987 435 002"
            };
            List<Onderdeel> onderdelen_auto12 = new List<Onderdeel>
            {
                onderdeel_auto12_1,onderdeel_auto12_2,onderdeel_auto12_3,onderdeel_auto12_4
            };
            //Auto13
            Onderdeel onderdeel_auto13_1 = new Onderdeel
            {
                Auto = auto13, Beschrijving = "Olie filter", Merk = "Bosch", BestelNr = "1 457 429 192"
            };
            Onderdeel onderdeel_auto13_2 = new Onderdeel
            {
                Auto = auto13, Beschrijving = "Remblokken voor", Merk = "Bosch", BestelNr = "0 986 494 019"
            };
            List<Onderdeel> onderdelen_auto13 = new List<Onderdeel>
            {
                onderdeel_auto13_1,onderdeel_auto13_2
            };
            //Auto15
            Onderdeel onderdeel_auto15_1 = new Onderdeel
            {
                Auto = auto15, Beschrijving = "Olie filter", Merk = "Bosch", BestelNr = "1 457 429 249"
            };
            Onderdeel onderdeel_auto15_2 = new Onderdeel
            {
                Auto = auto15, Beschrijving = "Lucht filter", Merk = "Bosch", BestelNr = "F 026 400 219"
            };
            Onderdeel onderdeel_auto15_3 = new Onderdeel
            {
                Auto = auto15, Beschrijving = "Bougie", Merk = "Bosch", BestelNr = "0 242 135 518"
            };
            Onderdeel onderdeel_auto15_4 = new Onderdeel
            {
                Auto = auto15, Beschrijving = "Interieur filter", Merk = "Bosch", BestelNr = "0 986 628 533"
            };
            Onderdeel onderdeel_auto15_5 = new Onderdeel
            {
                Auto = auto15, Beschrijving = "Wissers voor", Merk = "Bosch", BestelNr = "3 397 007 414"
            };
            Onderdeel onderdeel_auto15_6 = new Onderdeel
            {
                Auto = auto15, Beschrijving = "Wissers achter", Merk = "Bosch", BestelNr = "3 397 004 631"
            };
            Onderdeel onderdeel_auto15_7 = new Onderdeel
            {
                Auto = auto15, Beschrijving = "Waterpomp, multieriem, spanner set", Merk = "SKF", BestelNr = "VKMC 33410"
            };
            Onderdeel onderdeel_auto15_8 = new Onderdeel
            {
                Auto = auto15, Beschrijving = "Remblokken voor", Merk = "Bosch", BestelNr = "0 986 424 825"
            };
            Onderdeel onderdeel_auto15_9 = new Onderdeel
            {
                Auto = auto15, Beschrijving = "Rremschijf voor", Merk = "Bosch", BestelNr = "0 986 479 548"
            };
            Onderdeel onderdeel_auto15_10 = new Onderdeel
            {
                Auto = auto15, Beschrijving = "Poulie waterpomp", Merk = "Febi", BestelNr = "102466"
            };
            List<Onderdeel> onderdelen_auto15 = new List<Onderdeel>
            {
                onderdeel_auto15_1,onderdeel_auto15_2,onderdeel_auto15_3,onderdeel_auto15_4,onderdeel_auto15_5,
                onderdeel_auto15_6,onderdeel_auto15_7,onderdeel_auto15_8,onderdeel_auto15_9,onderdeel_auto15_10
            };
            //Alle onderdelen
            List<Onderdeel> alleOnderdelen = new List<Onderdeel>();
            alleOnderdelen.AddRange(onderdelen_auto1);
            alleOnderdelen.AddRange(onderdelen_auto2);
            alleOnderdelen.AddRange(onderdelen_auto5);
            alleOnderdelen.AddRange(onderdelen_auto7);
            alleOnderdelen.AddRange(onderdelen_auto8);
            alleOnderdelen.AddRange(onderdelen_auto9);
            alleOnderdelen.AddRange(onderdelen_auto10);
            alleOnderdelen.AddRange(onderdelen_auto11);
            alleOnderdelen.AddRange(onderdelen_auto12);
            alleOnderdelen.AddRange(onderdelen_auto13);
            alleOnderdelen.AddRange(onderdelen_auto15);

            //Werk
            Werk werk1 = new Werk
            {
                Auto = auto2, Datum = new DateTime(2020,6,11),  KilometerStand = 175000,OliefilterVervangen = true, Extra = "Remblokken achter"
            };
            Werk werk2 = new Werk
            {
                Auto = auto2, Datum = new DateTime(2020,06,11), OliefilterVervangen = true, KilometerStand = 185000
            };
            Werk werk3 = new Werk
            {
                Auto = auto1, KilometerStand = 130000, OliefilterVervangen = true, LuchtfilterVervangen = true, BrandstoffilterVervangen = true, InterieurfilterVervangen = true, Datum = new DateTime(2020,06,13), Extra = "Distributieriem set + waterpomp,  multi V riem,  vrijloop poulie  alternator,  stofkap cardan rv wielkant,  achterste draagarmrubbers L+R vooraan,   "
            };
            Werk werk4 = new Werk
            {
                Auto = auto8, KilometerStand = 91000, OliefilterVervangen = true, LuchtfilterVervangen = true, BrandstoffilterVervangen = true, InterieurfilterVervangen = true, Datum = new DateTime(2020,06,26), Extra = "Remblokken voor"
            };
            Werk werk5 = new Werk
            {
                Auto = auto9, KilometerStand = 166000, OliefilterVervangen = true, Datum = new DateTime(2020,06,28)
            };
            Werk werk6 = new Werk
            {
                Auto = auto10, KilometerStand = 125500, OliefilterVervangen = true, LuchtfilterVervangen = true, BrandstoffilterVervangen = true, InterieurfilterVervangen = true, Datum = new DateTime(2020,06,28)
            };
            Werk werk7 = new Werk
            {
                Auto = auto10, KilometerStand = 129500, OliefilterVervangen = true, Datum = new DateTime(2020,06,28), Extra = "Remblokken voor"
            };
            Werk werk8 = new Werk
            {
                Auto = auto6, KilometerStand = 188500, OliefilterVervangen = true, LuchtfilterVervangen = true, BrandstoffilterVervangen = true, InterieurfilterVervangen = true, Datum = new DateTime(2020,07,15)
            };
            Werk werk9 = new Werk
            {
                Auto = auto11, KilometerStand = 156500, OliefilterVervangen = true, Datum = new DateTime(2020,07,23)
            };
            Werk werk10 = new Werk
            {
                Auto = auto12, KilometerStand = 134300, OliefilterVervangen = true, Extra = "dpf drukverschil sensor", Datum = new DateTime(2020,07,25)
            };
            Werk werk11 = new Werk
            {
                Auto = auto13, KilometerStand = 141000, OliefilterVervangen = true, Datum = new DateTime(2020,07,25), Extra = "Remblokken voor"
            };
            Werk werk12 = new Werk
            {
                Auto = auto14, KilometerStand = 216800, OliefilterVervangen = true, Datum = new DateTime(2020,07,28)
            };
            Werk werk13 = new Werk
            {
                Auto = auto15, KilometerStand = 91000, OliefilterVervangen = true, LuchtfilterVervangen = true, Datum = new DateTime(2020, 8,9), Extra = "Batterij, banden, waterpomp, multie v riem, spanner, wissers voor, wisser achter, remblokken voor, bougies"
            };
            Werk werk14 = new Werk
            {
                Auto = auto7, KilometerStand = 121400, LuchtfilterVervangen = true, Datum = new DateTime(2020,08,12)
            };
            Werk werk15 = new Werk
            {
                Auto = auto15, KilometerStand = 91001, Datum = new DateTime(2020,8,17), Extra = "Remschijven voor, poulie waterpomp"
            };
            List<Werk> alleWerken = new List<Werk>
            {
                werk1, werk2, werk3, werk4, werk5, werk6, werk7, werk8, werk9, werk10, werk11, werk12, werk13, werk14, werk15
            };

            auto1.Onderdelen = onderdelen_auto1;
            auto1.WerkVoorAuto = new List<Werk>(new []{werk3});
            auto2.Onderdelen = onderdelen_auto2;
            auto2.WerkVoorAuto = new List<Werk>(new []{werk1, werk2});
            auto3.Onderdelen = new List<Onderdeel>();
            auto3.WerkVoorAuto = new List<Werk>();
            auto4.Onderdelen = new List<Onderdeel>();
            auto4.WerkVoorAuto = new List<Werk>();
            auto5.Onderdelen = onderdelen_auto5;
            auto5.WerkVoorAuto = new List<Werk>();
            auto6.Onderdelen = new List<Onderdeel>();
            auto6.WerkVoorAuto = new List<Werk>(new []{werk8});
            auto7.Onderdelen = onderdelen_auto7;
            auto7.WerkVoorAuto = new List<Werk>(new []{werk14});
            auto8.Onderdelen = onderdelen_auto8;
            auto8.WerkVoorAuto = new List<Werk>(new []{werk4});
            auto9.Onderdelen = onderdelen_auto9;
            auto9.WerkVoorAuto = new List<Werk>(new []{werk5});
            auto10.Onderdelen = onderdelen_auto10;
            auto10.WerkVoorAuto = new List<Werk>(new []{werk6, werk7});
            auto11.Onderdelen = onderdelen_auto11;
            auto11.WerkVoorAuto = new List<Werk>(new []{werk9});
            auto12.Onderdelen = onderdelen_auto12;
            auto12.WerkVoorAuto = new List<Werk>(new []{werk10});
            auto13.Onderdelen = onderdelen_auto13;
            auto13.WerkVoorAuto = new List<Werk>(new []{werk11});
            auto14.Onderdelen = new List<Onderdeel>();
            auto14.WerkVoorAuto = new List<Werk>(new []{werk12});
            auto15.Onderdelen = onderdelen_auto15;
            auto15.WerkVoorAuto = new List<Werk>(new []{werk13, werk15});

            //Alle autos incl onderdelen
            List<Auto> alleAutos = new List<Auto>(new []{auto1, auto2, auto3, auto4, auto5, auto6, auto7, auto8, auto9, auto10, auto11, auto12, auto13, auto14, auto15});

            //Alles naar Dbcontext:
            context.Onderdelen.AddRange(alleOnderdelen);
            context.Werken.AddRange(alleWerken);
            context.Autos.AddRange(alleAutos);

            context.SaveChanges();
            foreach (EntityEntry entry in context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }

        private static void SeedOrg(VandenEyndeDbContext context)
    {

        Onderdeel onderdeel = new Onderdeel()
        {
            Beschrijving = "Test onderdeel", Merk = "Bosch", BestelNr = "123-KVR-009", OnderdeelId = 1
        };
        Onderdeel onderdeel2 = new Onderdeel()
        {
            Beschrijving = "Test ", Merk = "Bosch", BestelNr = "1sd2sdsd3-KVR-009", OnderdeelId = 2
        };
        Onderdeel onderdeel3 = new Onderdeel()
        {
            Beschrijving = "Test dqsdqsdqsdqs", Merk = "Bosch", BestelNr = "123-Ksdsd-009", OnderdeelId = 3
        };
        // Initialize the basic data of the application and/or some dummy data
        Auto auto1 = new Auto()
        {
            Naam = "VW Polo 1.2 TDI",
            OlieInhoud = 12,
            Bouwjaar = 2005,
            CylinderInhoud = 1198,
            VermogenKw = 89,
            MotorCode = "VKVJDLKJFLKD24545",
            Banden = "r16 bka bla",
            Onderdelen = new List<Onderdeel>(),
            WerkVoorAuto = new List<Werk>()
        };
        Werk werk1 = new Werk()
        {
            Auto = auto1, Datum = DateTime.Now, InterieurfilterVervangen = true, KilometerStand = 91000, LuchtfilterVervangen = false, OliefilterVervangen = true, WerkId = 1, Extra = "Eerste service sinds 2017"
        };
        auto1.WerkVoorAuto.Add(werk1);
        onderdeel.Auto = auto1;
        onderdeel2.Auto = auto1;
        auto1.Onderdelen.Add(onderdeel);
        auto1.Onderdelen.Add(onderdeel2);
        Auto auto2 = new Auto()
        {
            Naam = "Peugeot 208 1.6 Sport",
            OlieInhoud = 12,
            Bouwjaar = 2012,
            CylinderInhoud = 1598,
            VermogenKw = 115,
            MotorCode = "VKVJDLKJFLKD24545",
            Banden = "r16 bka bla",
            Onderdelen = new List<Onderdeel>(),
            WerkVoorAuto = new List<Werk>()
        };
        onderdeel3.Auto = auto2;
        auto2.Onderdelen.Add(onderdeel3);

        Auto auto3 = new Auto()
        {
            Naam = "Peugeot 2008 1.6 Benzine",
            OlieInhoud = 12,
            Bouwjaar = 2012,
            CylinderInhoud = 1598,
            VermogenKw = 100,
            MotorCode = "VKVJDLKJFLKD24545",
            Banden = "r16 bka bla",
            WerkVoorAuto = new List<Werk>(),
            Onderdelen = new List<Onderdeel>()
        };

        context.Onderdelen.Add(onderdeel);
        context.Onderdelen.Add(onderdeel2);
        context.Onderdelen.Add(onderdeel3);
        context.Werken.Add(werk1);
context.Autos.Add(auto1);
        context.Autos.Add(auto2);
        context.Autos.Add(auto3);
        context.SaveChanges();
        foreach (EntityEntry entry in context.ChangeTracker.Entries().ToList())
        {
            entry.State = EntityState.Detached;
        }

    }
        */
    }
}