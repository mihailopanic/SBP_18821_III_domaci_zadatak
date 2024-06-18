using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SahFederacijaLibrary.Entiteti;
using FluentNHibernate.Mapping;
using FluentNHibernate.Conventions.Helpers;

namespace SahFederacijaLibrary.Mapiranja
{
    public class SahistaMapiranja : ClassMap<SahFederacijaLibrary.Entiteti.Sahista>
    {
        public SahistaMapiranja()
        { 
            Table("SAHISTA");

            // ID se generise pomocu sekvence
            Id(x => x.Rbr, "RBR").GeneratedBy.SequenceIdentity("SAHISTA_SEQ");

            Map(x => x.Zemlja_Porekla, "ZEMLJA_POREKLA");
            Map(x => x.Broj_Pasosa, "BROJ_PASOSA");
            Map(x => x.Datum_Uclanjenja, "DATUM_UCLANJENJA");
            Map(x => x.Lime, "LIME");
            Map(x => x.Sslovo, "SSLOVO");
            Map(x => x.Prezime, "PREZIME");
            Map(x => x.Adresa, "ADRESA");
            Map(x => x.Datum_Rodjenja, "DATUM_RODJENJA");
            Map(x => x.Status, "STATUS");

            // mapiranje veze M:N izmedju SAHISTA i SAHOVSKI_TURNIR
            /* HasManyToMany(x => x.SahovskiTurniri)
                .Table("UCESTVUJE_NA")
                .ParentKeyColumn("RBR_SAHISTA")
                .ChildKeyColumn("ID_SAHOVSKI_TURNIR")
                .Cascade.All();
            */
            HasMany(x => x.UcestvujeNaSahovski_Turnir).KeyColumn("RBR_SAHISTA").LazyLoad().Cascade.All().Inverse();

            // mapiranje veze M:N izmedju SAHISTA i PARTIJA
            /* HasManyToMany(x => x.Partije)
                .Table("IGRA")
                .ParentKeyColumn("RBR_SAHISTA")
                .ChildKeyColumn("ID_PARTIJA")
                .Cascade.All()
                .Inverse();
            */
            HasMany(x => x.IgraPartija).KeyColumn("RBR_SAHISTA").LazyLoad().Cascade.All().Inverse();
        }
    }
}
