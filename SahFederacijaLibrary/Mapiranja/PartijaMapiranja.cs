using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using SahFederacijaLibrary.Entiteti;

namespace SahFederacijaLibrary.Mapiranja
{
    public class PartijaMapiranja : ClassMap<SahFederacijaLibrary.Entiteti.Partija>
    {
        public PartijaMapiranja()
        {
            Table("PARTIJA");

            // ID se generise pomocu sekvence
            Id(x => x.Id, "ID").GeneratedBy.SequenceIdentity("PARTIJA_SEQ");

            Map(x => x.Datum_Vreme_Odigravanja, "DATUM_VREME_ODIGRAVANJA");
            Map(x => x.Rezultat_Partije, "REZULTAT_PARTIJE");
            Map(x => x.Trajanje_Partije, "TRAJANJE_PARTIJE");

            // mapiranje stranog kljuca sahiste koji je igrao crnim/belim figurama
            References(x => x.Crne_Figure).Column("CRNE_FIGURE").LazyLoad();
            References(x => x.Bele_Figure).Column("BELE_FIGURE").LazyLoad();

            // mapiranje veze N:1 izmedju PARTIJA i SAHOVSKI_TURNIR
            References(x => x.SahovskiTurnir).Column("ID_SAHOVSKI_TURNIR").LazyLoad();

            // mapiranje veze N:1 izmedju PARTIJA i SUDIJA
            References(x => x.Sudija).Column("ID_SUDIJA").LazyLoad();

            // mapiranje veze 1:N izmedju PARTIJA i POTEZ (slabi tip entiteta)
            HasMany(x => x.Potezi).KeyColumn("ID_PARTIJA").Cascade.All().Inverse();

            // mapiranje veze M:N izmedju PARTIJA i SAHISTA 
            /*HasManyToMany(x => x.Sahiste)
                .Table("IGRA")
                .ParentKeyColumn("ID_PARTIJA")
                .ChildKeyColumn("RBR_SAHISTA")
                .Cascade.All();
            */
            HasMany(x => x.SahistaIgra).KeyColumn("ID_PARTIJA").LazyLoad().Cascade.All().Inverse();
        }
    }
}
