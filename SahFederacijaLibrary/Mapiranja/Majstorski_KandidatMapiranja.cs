using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SahFederacijaLibrary.Entiteti;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions.Helpers;

namespace SahovskaFederacija.Mapiranja
{
    public class Majstorski_KandidatMapiranja : SubclassMap<SahFederacijaLibrary.Entiteti.Majstorski_Kandidat>
    {
        public Majstorski_KandidatMapiranja()
        {
            Table("MAJSTORSKI_KANDIDAT");

            // Primarni kljuc nasledjen iz klase Sahista
            KeyColumn("RBR");

            Map(x => x.Broj_Partija_Do_Zvanja).Column("BROJ_PARTIJA_DO_ZVANJA");
        }
    }
}
